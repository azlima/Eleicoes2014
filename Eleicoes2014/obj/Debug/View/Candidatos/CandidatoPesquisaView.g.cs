﻿#pragma checksum "C:\Dvp\My Apps\Eleicoes2014 - 2º Turno\Eleicoes2014\View\Candidatos\CandidatoPesquisaView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6E954A2F3547E81D795F04DAC6FE88F2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
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


namespace Eleicoes2014.View.Candidatos {
    
    
    public partial class CandidatoPesquisaView : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel principalStackPanel;
        
        internal System.Windows.Controls.Button partidosBotao;
        
        internal System.Windows.Controls.TextBox numeroOuNomeTextBox;
        
        internal System.Windows.Controls.Image pesquisarImagem;
        
        internal System.Windows.Controls.ListBox partidosListagem;
        
        internal System.Windows.Controls.TextBlock displayProgresso;
        
        internal System.Windows.Controls.ProgressBar barraProgresso;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Eleicoes2014;component/View/Candidatos/CandidatoPesquisaView.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.principalStackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("principalStackPanel")));
            this.partidosBotao = ((System.Windows.Controls.Button)(this.FindName("partidosBotao")));
            this.numeroOuNomeTextBox = ((System.Windows.Controls.TextBox)(this.FindName("numeroOuNomeTextBox")));
            this.pesquisarImagem = ((System.Windows.Controls.Image)(this.FindName("pesquisarImagem")));
            this.partidosListagem = ((System.Windows.Controls.ListBox)(this.FindName("partidosListagem")));
            this.displayProgresso = ((System.Windows.Controls.TextBlock)(this.FindName("displayProgresso")));
            this.barraProgresso = ((System.Windows.Controls.ProgressBar)(this.FindName("barraProgresso")));
        }
    }
}

