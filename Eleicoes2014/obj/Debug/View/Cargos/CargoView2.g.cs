﻿#pragma checksum "C:\Dvp\Pjt\Testes WP\My Apps\Eleicoes2014\Eleicoes2014\View\Cargos\CargoView2.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "13ED1C152E4DD0C9528FF5952BDE16EE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace Eleicoes2014.View.Cargos {
    
    
    public partial class CargoView : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel CargosListagemStackPanel;
        
        internal System.Windows.Controls.ListBox cargosListagem;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Eleicoes2014;component/View/Cargos/CargoView2.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.CargosListagemStackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("CargosListagemStackPanel")));
            this.cargosListagem = ((System.Windows.Controls.ListBox)(this.FindName("cargosListagem")));
        }
    }
}

