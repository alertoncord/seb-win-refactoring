﻿/*
 * Copyright (c) 2023 ETH Zürich, IT Services
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using CefSharp;

namespace SafeExamBrowser.Browser.Wrapper.Events
{
	internal delegate void AuthCredentialsEventHandler(IWebBrowser webBrowser, IBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback, GenericEventArgs args);
}
