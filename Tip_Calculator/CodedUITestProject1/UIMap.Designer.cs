﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by coded UI test builder.
//      Version: 14.0.0.0
//
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------

namespace CodedUITestProject1
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using Microsoft.VisualStudio.TestTools.UITest.Extension;
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using MouseButtons = System.Windows.Forms.MouseButtons;
    
    
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public partial class UIMap
    {
        
        /// <summary>
        /// Opens, checks tip for 5 and 20 works
        /// </summary>
        public void TestBasic1()
        {
            #region Variable Declarations
            WinEdit uITotalBoxEdit = this.UIEnterTotalBillWindow.UITotalBoxWindow.UITotalBoxEdit;
            #endregion

            // Launch '%USERPROFILE%\Documents\kb\fall 2016\cs3500\assignments\Tip_Calculator\Tip_Calculator\bin\Debug\Tip_Calculator.exe'
            ApplicationUnderTest uIEnterTotalBillWindow = ApplicationUnderTest.Launch(this.TestBasic1Params.UIEnterTotalBillWindowExePath, this.TestBasic1Params.UIEnterTotalBillWindowAlternateExePath);

            // Type '5' in 'totalBox' text box
            uITotalBoxEdit.Text = this.TestBasic1Params.UITotalBoxEditText;

            // Type '20' in 'totalBox' text box
            uITotalBoxEdit.Text = this.TestBasic1Params.UITotalBoxEditText1;
        }
        
        /// <summary>
        /// assert bill is 24
        /// </summary>
        public void AssertValue1()
        {
            #region Variable Declarations
            WinEdit uITotalAmountBoxEdit = this.UIEnterTotalBillWindow.UITotalAmountBoxWindow.UITotalAmountBoxEdit;
            #endregion

            // Verify that the 'Text' property of 'totalAmountBox' text box equals '24.00'
            Assert.AreEqual(this.AssertValue1ExpectedValues.UITotalAmountBoxEditText, uITotalAmountBoxEdit.Text, "Expected 24.00");
        }
        
        #region Properties
        public virtual TestBasic1Params TestBasic1Params
        {
            get
            {
                if ((this.mTestBasic1Params == null))
                {
                    this.mTestBasic1Params = new TestBasic1Params();
                }
                return this.mTestBasic1Params;
            }
        }
        
        public virtual AssertValue1ExpectedValues AssertValue1ExpectedValues
        {
            get
            {
                if ((this.mAssertValue1ExpectedValues == null))
                {
                    this.mAssertValue1ExpectedValues = new AssertValue1ExpectedValues();
                }
                return this.mAssertValue1ExpectedValues;
            }
        }
        
        public UIEnterTotalBillWindow UIEnterTotalBillWindow
        {
            get
            {
                if ((this.mUIEnterTotalBillWindow == null))
                {
                    this.mUIEnterTotalBillWindow = new UIEnterTotalBillWindow();
                }
                return this.mUIEnterTotalBillWindow;
            }
        }
        #endregion
        
        #region Fields
        private TestBasic1Params mTestBasic1Params;
        
        private AssertValue1ExpectedValues mAssertValue1ExpectedValues;
        
        private UIEnterTotalBillWindow mUIEnterTotalBillWindow;
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'TestBasic1'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class TestBasic1Params
    {
        
        #region Fields
        /// <summary>
        /// Launch '%USERPROFILE%\Documents\kb\fall 2016\cs3500\assignments\Tip_Calculator\Tip_Calculator\bin\Debug\Tip_Calculator.exe'
        /// </summary>
        public string UIEnterTotalBillWindowExePath = "C:\\Users\\Karina\\Documents\\kb\\fall 2016\\cs3500\\assignments\\Tip_Calculator\\Tip_Calc" +
            "ulator\\bin\\Debug\\Tip_Calculator.exe";
        
        /// <summary>
        /// Launch '%USERPROFILE%\Documents\kb\fall 2016\cs3500\assignments\Tip_Calculator\Tip_Calculator\bin\Debug\Tip_Calculator.exe'
        /// </summary>
        public string UIEnterTotalBillWindowAlternateExePath = "%USERPROFILE%\\Documents\\kb\\fall 2016\\cs3500\\assignments\\Tip_Calculator\\Tip_Calcul" +
            "ator\\bin\\Debug\\Tip_Calculator.exe";
        
        /// <summary>
        /// Type '5' in 'totalBox' text box
        /// </summary>
        public string UITotalBoxEditText = "5";
        
        /// <summary>
        /// Type '20' in 'totalBox' text box
        /// </summary>
        public string UITotalBoxEditText1 = "20";
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'AssertValue1'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class AssertValue1ExpectedValues
    {
        
        #region Fields
        /// <summary>
        /// Verify that the 'Text' property of 'totalAmountBox' text box equals '24.00'
        /// </summary>
        public string UITotalAmountBoxEditText = "24.00";
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class UIEnterTotalBillWindow : WinWindow
    {
        
        public UIEnterTotalBillWindow()
        {
            #region Search Criteria
            this.SearchProperties[WinWindow.PropertyNames.Name] = "Enter Total Bill:";
            this.SearchProperties.Add(new PropertyExpression(WinWindow.PropertyNames.ClassName, "WindowsForms10.Window", PropertyExpressionOperator.Contains));
            this.WindowTitles.Add("Enter Total Bill:");
            #endregion
        }
        
        #region Properties
        public UITotalBoxWindow UITotalBoxWindow
        {
            get
            {
                if ((this.mUITotalBoxWindow == null))
                {
                    this.mUITotalBoxWindow = new UITotalBoxWindow(this);
                }
                return this.mUITotalBoxWindow;
            }
        }
        
        public UITotalAmountBoxWindow UITotalAmountBoxWindow
        {
            get
            {
                if ((this.mUITotalAmountBoxWindow == null))
                {
                    this.mUITotalAmountBoxWindow = new UITotalAmountBoxWindow(this);
                }
                return this.mUITotalAmountBoxWindow;
            }
        }
        #endregion
        
        #region Fields
        private UITotalBoxWindow mUITotalBoxWindow;
        
        private UITotalAmountBoxWindow mUITotalAmountBoxWindow;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class UITotalBoxWindow : WinWindow
    {
        
        public UITotalBoxWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[WinWindow.PropertyNames.ControlName] = "totalBox";
            this.WindowTitles.Add("Enter Total Bill:");
            #endregion
        }
        
        #region Properties
        public WinEdit UITotalBoxEdit
        {
            get
            {
                if ((this.mUITotalBoxEdit == null))
                {
                    this.mUITotalBoxEdit = new WinEdit(this);
                    #region Search Criteria
                    this.mUITotalBoxEdit.WindowTitles.Add("Enter Total Bill:");
                    #endregion
                }
                return this.mUITotalBoxEdit;
            }
        }
        #endregion
        
        #region Fields
        private WinEdit mUITotalBoxEdit;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class UITotalAmountBoxWindow : WinWindow
    {
        
        public UITotalAmountBoxWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[WinWindow.PropertyNames.ControlName] = "totalAmountBox";
            this.WindowTitles.Add("Enter Total Bill:");
            #endregion
        }
        
        #region Properties
        public WinEdit UITotalAmountBoxEdit
        {
            get
            {
                if ((this.mUITotalAmountBoxEdit == null))
                {
                    this.mUITotalAmountBoxEdit = new WinEdit(this);
                    #region Search Criteria
                    this.mUITotalAmountBoxEdit.WindowTitles.Add("Enter Total Bill:");
                    #endregion
                }
                return this.mUITotalAmountBoxEdit;
            }
        }
        #endregion
        
        #region Fields
        private WinEdit mUITotalAmountBoxEdit;
        #endregion
    }
}
