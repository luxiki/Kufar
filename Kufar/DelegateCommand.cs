﻿
    /****************************** Module Header ******************************\ 
    * Module Name:    DelegateCommand.cs 
    * Project:        CSUnvsAppCommandBindInDT 
    * Copyright (c) Microsoft Corporation. 
    *  
    * This class implements ICommand interface 
    *  
    * This source is subject to the Microsoft Public License. 
    * See http://www.microsoft.com/en-us/openness/licenses.aspx#MPL. 
    * All other rights reserved. 
    *  
    * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,  
    * EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED  
    * WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE. 
   \***************************************************************************/
    using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

    namespace HelperClass
    {
        public class DelegateCommand : ICommand
        {
            private Action<object> execute;
            private Func<object, bool> canExecute;

            public DelegateCommand(Action<object> execute)
            {
                this.execute = execute;
                this.canExecute = (x) => { return true; };
            }
            public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
            {
                this.execute = execute;
                this.canExecute = canExecute;
            }
            public bool CanExecute(object parameter)
            {
                return canExecute(parameter);
            }

            public event EventHandler CanExecuteChanged;
            public void RaiseCanExecuteChanged()
            {
                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, EventArgs.Empty);
                }
            }
            public void Execute(object parameter)
            {
                execute(parameter);
            }
        }


    public static class WebBrowserUtility
    {
        public static readonly DependencyProperty BindableSourceProperty =
            DependencyProperty.RegisterAttached("BindableSource", typeof(string), typeof(WebBrowserUtility), new UIPropertyMetadata(null, BindableSourcePropertyChanged));

        public static string GetBindableSource(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableSourceProperty);
        }

        public static void SetBindableSource(DependencyObject obj, string value)
        {
            obj.SetValue(BindableSourceProperty, value);
        }

        public static void BindableSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            WebBrowser browser = o as WebBrowser;
            if (browser != null)
            {
                string uri = e.NewValue as string;
                browser.Source = !String.IsNullOrEmpty(uri) ? new Uri(uri) : null;
            }
        }

    }






}

