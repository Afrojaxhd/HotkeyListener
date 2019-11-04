﻿#region Copyright

/*
 * Developer    : Willy Kimura (WK).
 * Library      : HotkeyListener.
 * License      : MIT.
 * 
 * I've had the privilege of building "pervasive" Desktop 
 * applications for products of my own. However, one of the 
 * key features required in most of them was the ability to 
 * invoke features whenever a user triggered a certain key or 
 * combination of keys. After looking around, I found one really 
 * functional library, "SmartHotkey", and it worked really well. 
 * However, there was a need for some few additional features 
 * in my products which led me to rebuilding the project and 
 * improving it even further. And thus came "HotkeyListener". 
 * 
 * This project combines two open-source libraries:
 * 
 *  (1) SmartHotKey: https://www.codeproject.com/Articles/100199/Smart-Hotkey-Handler-NET
 *  (2) HotkeySelection Control: https://www.codeproject.com/Articles/15085/A-simple-hotkey-selection-control-for-NET
 *  
 *  I've added some few improvements such as:
 *  
 *  (1) Ability to suspend and resume the list of hotkeys registered.
 *  (2) Ability to fetch source application info from where a hotkey is triggered.
 *  (3) Ability to enable any Windows control to provide Hotkey selection features.
 * 
 * Improvements are welcome.
 * 
 */

#endregion


using System;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;

using WK.Libraries.HotkeyListenerNS.Models;
using WK.Libraries.HotkeyListenerNS.Helpers;

namespace WK.Libraries.HotkeyListenerNS
{
    /// <summary>
    /// A library that provides support for registering and 
    /// attaching events to global hotkeys in .NET applications.
    /// </summary>
    [DebuggerStepThrough]
    public class HotkeyListener
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HotkeyListener"/> class.
        /// </summary>
        public HotkeyListener()
        {
            SetDefaults();
        }

        #endregion

        #region Fields

        // This is the handle that will be used to register, 
        // unregister, and listen to the hotkey triggers.
        private HotkeyHandle _handle = new HotkeyHandle();

        // Saves the list of hotkeys suspended.
        private Dictionary<int, string> _suspendedKeys = 
            new Dictionary<int, string>();
        
        // Saves the list of forms suspended.
        private List<Form> _suspendedForms = new List<Form>();

        #endregion

        #region Properties

        #region Public

        /// <summary>
        /// Gets a value determining whether the 
        /// hotkeys set have been suspended.
        /// </summary>
        public bool Suspended { get; private set; }
        
        #endregion

        #endregion

        #region Methods

        #region Public

        /// <summary>
        /// Adds a hotkey to the global Key watcher.
        /// </summary>
        /// <param name="hotkey">The hotkey to add.</param>
        public void Add(string hotkey)
        {
            _handle.AddKey(hotkey);
        }

        /// <summary>
        /// Adds a list of hotkeys to the global Key watcher.
        /// </summary>
        /// <param name="hotkeys">The hotkeys to add.</param>
        public void Add(string[] hotkeys)
        {
            foreach (string key in hotkeys)
            {
                Add(key);
            }
        }

        /// <summary>
        /// Updates an existing hotkey 
        /// in the global Key watcher.
        /// </summary>
        /// <param name="currentHotkey">The hotkey to modify.</param>
        /// <param name="newHotkey">The new hotkey to be set.</param>
        public void Update(string currentHotkey, string newHotkey)
        {
            try
            {
                if (!Suspended)
                {
                    Remove(currentHotkey);
                    Add(newHotkey);
                }
                else
                {
                    foreach (var item in _suspendedKeys.ToList())
                    {
                        if (item.Value == currentHotkey)
                        {
                            int keyID = item.Key;

                            _suspendedKeys.Remove(item.Key);
                            _suspendedKeys.Add(keyID, newHotkey);
                        }
                    }
                }

                currentHotkey = newHotkey;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Updates an existing hotkey 
        /// in the global Key watcher.
        /// </summary>
        /// <param name="currentHotkey">
        /// A reference to the variable 
        /// containing the hotkey to modify.
        /// </param>
        /// <param name="newHotkey">
        /// The new hotkey to be set.
        /// </param>
        public void Update(ref string currentHotkey, string newHotkey)
        {
            try
            {
                if (!Suspended)
                {
                    Remove(currentHotkey);
                    Add(newHotkey);
                }
                else
                {
                    foreach (var item in _suspendedKeys.ToList())
                    {
                        if (item.Value == currentHotkey)
                        {
                            int keyID = item.Key;

                            _suspendedKeys.Remove(item.Key);
                            _suspendedKeys.Add(keyID, newHotkey);
                        }
                    }
                }

                currentHotkey = newHotkey;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Updates an existing hotkey 
        /// in the global Key watcher.
        /// </summary>
        /// <param name="currentHotkey">
        /// A reference to the variable 
        /// containing the hotkey to modify.
        /// </param>
        /// <param name="newHotkey">
        /// A reference to the variable containing 
        /// the new hotkey to be set.
        /// </param>
        public void Update(ref string currentHotkey, ref string newHotkey)
        {
            try
            {
                if (!Suspended)
                {
                    Remove(currentHotkey);
                    Add(newHotkey);
                }
                else
                {
                    foreach (var item in _suspendedKeys.ToList())
                    {
                        if (item.Value == currentHotkey)
                        {
                            int keyID = item.Key;

                            _suspendedKeys.Remove(item.Key);
                            _suspendedKeys.Add(keyID, newHotkey);
                        }
                    }
                }

                currentHotkey = newHotkey;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Removes any specific hotkey 
        /// from the global Key watcher.
        /// </summary>
        /// <param name="hotkey">The hotkey to remove.</param>
        public void Remove(string hotkey)
        {
            _handle.RemoveKey(hotkey);
        }

        /// <summary>
        /// Removes a list of hotkeys from 
        /// the global Key watcher.
        /// </summary>
        /// <param name="hotkeys">The hotkeys to remove.</param>
        public void Remove(string[] hotkeys)
        {
            foreach (string key in hotkeys)
            {
                Remove(key);
            }
        }

        /// <summary>
        /// Remove all the registered hotkeys 
        /// from the global Key watcher.
        /// </summary>
        public void RemoveAll()
        {
            _handle.RemoveAllKeys();
        }

        /// <summary>
        /// Suspends the hotkey(s) set 
        /// in the global Key watcher.
        /// </summary>
        public void Suspend()
        {
            if (!Suspended)
            {
                foreach (var item in _handle.Hotkeys)
                {
                    _suspendedKeys.Add(item.Key, item.Value);
                }

                foreach (var key in _handle.Hotkeys.Values.ToList())
                {
                    Remove(key);
                }

                Suspended = true;
            }
        }

        /// <summary>
        /// Suspends the hotkey(s) set whenever a particular Form is active. 
        /// This is useful in Forms where the user requires modifying certain 
        /// hotkeys without triggering them when active.
        /// </summary>
        /// <param name="form">
        /// The Form to suspend listening to hotkeys when active.
        /// </param>
        public void SuspendOn(Form form)
        {
            try
            {
                form.Activated += OnActivateForm;
                form.Deactivate += OnDeactivateForm;
                
                _suspendedForms.Add(form);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Suspends the hotkey(s) set whenever a list of Forms are active. 
        /// This is useful in Forms where the user requires modifying certain 
        /// hotkeys without triggering them when active.
        /// </summary>
        /// <param name="form">
        /// The Forms to suspend listening to hotkeys when active.
        /// </param>
        public void SuspendOn(Form[] forms)
        {
            try
            {
                foreach (var form in forms)
                {
                    SuspendOn(form);
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Releases a Form from suspending hotkeys when active.
        /// </summary>
        /// <param name="form">
        /// The Form to resume to listening to hotkeys when active.
        /// </param>
        public void ResumeOn(Form form)
        {
            try
            {
                if (_suspendedForms != null)
                {
                    foreach (var addedForm in _suspendedForms)
                    {
                        if (addedForm.GetHashCode() == form.GetHashCode())
                        {
                            _suspendedForms.Remove(addedForm);

                            addedForm.Activated -= OnActivateForm;
                            addedForm.Deactivate -= OnDeactivateForm;
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Releases a list of Forms from suspending hotkeys when active.
        /// </summary>
        /// <param name="form">
        /// The Forms to resume to listening to hotkeys when active.
        /// </param>
        public void ResumeOn(Form[] forms)
        {
            try
            {
                foreach (var form in forms)
                {
                    ResumeOn(form);
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Resumes using the hotkey(s) that 
        /// were set in the global Key watcher.
        /// </summary>
        public void Resume()
        {
            if (Suspended)
            {
                foreach (var key in _suspendedKeys.Values.ToList())
                {
                    Add(key);
                }
            }
        }

        /// <summary>
        /// [Special] Gets the currently selected text 
        /// in any active application.
        /// </summary>
        /// <returns>The selected text, if any.</returns>
        public string GetSelection()
        {
            try
            {
                string clipboardText = Clipboard.GetText();
                SendKeys.SendWait("^(c)");

                System.Threading.Thread.Sleep(200);

                string selection = Clipboard.GetText();
                Clipboard.SetText(clipboardText);

                return selection;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// Applies the library-default options & settings.
        /// </summary>
        private void SetDefaults()
        {
            AttachEvents();
        }

        /// <summary>
        /// Attaches the major hotkey events 
        /// to the Hotkey Listener.
        /// </summary>
        private void AttachEvents()
        {
            _handle.HotkeyPressed += (s, e) =>
            {
                HotkeyPressed?.Invoke(
                    new SourceApplication(
                        SourceAttributes.GetID(),
                        SourceAttributes.GetHandle(),
                        SourceAttributes.GetName(),
                        SourceAttributes.GetTitle(),
                        SourceAttributes.GetPath()),
                    new HotkeyEventArgs
                    {
                        Hotkey = e.Hotkey,
                        SourceApplication = new SourceApplication(
                        SourceAttributes.GetID(),
                        SourceAttributes.GetHandle(),
                        SourceAttributes.GetName(),
                        SourceAttributes.GetTitle(),
                        SourceAttributes.GetPath())
                    });
            };
        }
        
        #endregion

        #endregion

        #region Events

        #region Public

        /// <summary>
        /// Raised whenever a registered Hotkey is pressed.
        /// </summary>
        public event HotkeyEventHandler HotkeyPressed;
    
        /// <summary>
        /// Represents the method that will handle a <see cref="HotkeyPressed"/> 
        /// event that has no event data.
        /// </summary>
        /// <param name="sender">The hotkey sender object.</param>
        /// <param name="e">The <see cref="HotkeyEventArgs"/> data.</param>
        public delegate void HotkeyEventHandler(object sender, HotkeyEventArgs e);

        #endregion

        #region Private

        private void OnActivateForm(object sender, EventArgs e)
        {
            Suspend();
        }

        private void OnDeactivateForm(object sender, EventArgs e)
        {
            Resume();
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Provides data for the <see cref="HotkeyListener.HotkeyPressed"/> event.
    /// </summary>
    public class HotkeyEventArgs : EventArgs
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HotkeyEventArgs"/> class.
        /// </summary>
        public HotkeyEventArgs() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HotkeyEventArgs"/> class.
        /// </summary>
        /// <param name="source">
        /// The source application from where 
        /// the Hotkey was triggered.
        /// </param>
        public HotkeyEventArgs(SourceApplication source)
        {
            SourceApplication = new SourceApplication(
                source.ID, source.Handle, source.Name,
                source.Title, source.Path);
        }

        #endregion
        
        #region Properties

        /// <summary>
        /// Gets the hotkey that was pressed.
        /// </summary>
        public string Hotkey { get; internal set; }

        /// <summary>
        /// Gets the details of the source application 
        /// from where the hotkey was triggered.
        /// </summary>
        public SourceApplication SourceApplication { get; internal set; }

        #endregion
    }
}