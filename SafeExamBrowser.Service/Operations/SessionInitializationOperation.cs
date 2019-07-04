﻿/*
 * Copyright (c) 2019 ETH Zürich, Educational Development and Technology (LET)
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Threading;
using SafeExamBrowser.Contracts.Core.OperationModel;
using SafeExamBrowser.Contracts.Logging;

namespace SafeExamBrowser.Service.Operations
{
	internal class SessionInitializationOperation : SessionOperation
	{
		private ILogger logger;
		private Func<string, EventWaitHandle> serviceEventFactory;

		public SessionInitializationOperation(ILogger logger, Func<string, EventWaitHandle> serviceEventFactory, SessionContext sessionContext) : base(sessionContext)
		{
			this.logger = logger;
			this.serviceEventFactory = serviceEventFactory;
		}

		public override OperationResult Perform()
		{
			logger.Info("Initializing new session...");
			logger.Info($" -> Client-ID: {Context.Configuration.AppConfig.ClientId}");
			logger.Info($" -> Runtime-ID: {Context.Configuration.AppConfig.RuntimeId}");
			logger.Info($" -> Session-ID: {Context.Configuration.SessionId}");

			logger.Info("Stopping auto-restore mechanism...");
			Context.AutoRestoreMechanism.Stop();

			InitializeServiceEvent();

			return OperationResult.Success;
		}

		public override OperationResult Revert()
		{
			var success = true;

			logger.Info("Finalizing current session...");

			if (Context.ServiceEvent != null && Context.IsRunning)
			{
				success = Context.ServiceEvent.Set();

				if (success)
				{
					logger.Info("Successfully informed runtime about session termination.");
				}
				else
				{
					logger.Error("Failed to inform runtime about session termination!");
				}
			}

			logger.Info("Starting auto-restore mechanism...");
			Context.AutoRestoreMechanism.Start();

			logger.Info("Clearing session data...");
			Context.Configuration = null;
			Context.IsRunning = false;

			return success ? OperationResult.Success : OperationResult.Failed;
		}

		private void InitializeServiceEvent()
		{
			if (Context.ServiceEvent != null)
			{
				logger.Info("Closing service event from previous session...");
				Context.ServiceEvent.Close();
				logger.Info("Service event successfully closed.");
			}

			logger.Info("Attempting to create new service event...");
			Context.ServiceEvent = serviceEventFactory.Invoke(Context.Configuration.AppConfig.ServiceEventName);
			logger.Info("Service event successfully created.");
		}
	}
}
