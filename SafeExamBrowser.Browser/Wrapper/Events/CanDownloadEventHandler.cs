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
	internal delegate void CanDownloadEventHandler(IWebBrowser webBrowser, IBrowser browser, string url, string requestMethod, GenericEventArgs args);
}
