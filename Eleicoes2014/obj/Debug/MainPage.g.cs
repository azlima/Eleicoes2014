﻿#pragma checksum "C:\Dvp\My Apps\Eleicoes2014 - 2º Turno\Eleicoes2014\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0E1222FBCF6EA75054752184AC339622"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Eleicoes2014.View.Cargos;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Eleicoes2014 {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal Eleicoes2014.View.Cargos.CargoView cargoView;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton atualizarBotao;
        
        internal Microsoft.Phone.Shell.ApplicationBarMenuItem classificarMenuItem;
        
        internal Microsoft.Phone.Shell.ApplicationBarMenuItem contribuirMenuItem;
        
        internal Microsoft.Phone.Shell.ApplicationBarMenuItem falarDesenvolvedorMenuItem;
        
        internal Microsoft.Phone.Shell.ApplicationBarMenuItem sobreMenuItem;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Eleicoes2014;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.cargoView = ((Eleicoes2014.View.Cargos.CargoView)(this.FindName("cargoView")));
            this.atualizarBotao = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("atualizarBotao")));
            this.classificarMenuItem = ((Microsoft.Phone.Shell.ApplicationBarMenuItem)(this.FindName("classificarMenuItem")));
            this.contribuirMenuItem = ((Microsoft.Phone.Shell.ApplicationBarMenuItem)(this.FindName("contribuirMenuItem")));
            this.falarDesenvolvedorMenuItem = ((Microsoft.Phone.Shell.ApplicationBarMenuItem)(this.FindName("falarDesenvolvedorMenuItem")));
            this.sobreMenuItem = ((Microsoft.Phone.Shell.ApplicationBarMenuItem)(this.FindName("sobreMenuItem")));
        }
    }
}

