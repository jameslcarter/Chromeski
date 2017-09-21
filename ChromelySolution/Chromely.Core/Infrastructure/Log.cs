﻿/**
 MIT License

 Copyright (c) 2017 Kola Oyewumi

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 SOFTWARE.
 */

namespace Chromely.Core.Infrastructure
{
    using System;

    public static class Log
    {
        static string m_loggerFile = null;
        static IChromelyLogger m_logger = null;
        static bool m_logToConsole = true;

        public static IChromelyLogger Logger
        {
            get
            {
                if (m_logger == null)
                {
                    m_logger = new SimpleLogger(m_loggerFile, m_logToConsole);
                }

                return m_logger;
            }
            set
            {
                m_logger = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        public static string LoggerFile
        {
            get { return m_loggerFile; }
            set
            {
                m_loggerFile = value;
            }
        }

        public static bool LogToConsole
        {
            get { return m_logToConsole; }
            set
            {
                m_logToConsole = value;
            }
        }

        public static void Info(string message)
        {
            Logger.Info(message);
        }

        public static void Debug(string message)
        {
            Logger.Debug(message);
        }

        public static void Verbose(string message)
        {
            Logger.Verbose(message);
        }

        public static void Warn(string message)
        {
            Logger.Warn(message);
        }

        public static void Error(string message)
        {
            Logger.Error(message);
        }

        public static void Error(Exception exception, string message = null)
        {
            Logger.Error(exception, message);
        }

        public static void Fatal(string message)
        {
            Logger.Fatal(message);
        }

        public static void Critial(string message)
        {
            Logger.Fatal(message);
        }
    }
}