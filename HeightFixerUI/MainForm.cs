﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HeightFixerLibrary;
using System.IO;

namespace HeightFixerUI
{
    public partial class MainForm : Form
    {
        ConverterModel model = new ConverterModel();

        public MainForm()
        {
            InitializeComponent();
            inputFileValue.Text = "";
            outputFileValue.Text = "";
        }

        private void browseInputButton_Click(object sender, EventArgs e)
        {
            SetInput();        
        }

        private void outputBrowseButton_Click(object sender, EventArgs e)
        {
            SetOutput(model.InputPath);
        }

        private void SetInput()
        {
            OpenFileDialog selectInputFile = new OpenFileDialog();
            selectInputFile.Title = "Select file...";
            selectInputFile.InitialDirectory = @"::{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
            selectInputFile.Filter = ".nmea (*.nmea)|*.nmea";
            selectInputFile.FilterIndex = 1;
            selectInputFile.RestoreDirectory = true;
            if (selectInputFile.ShowDialog() == DialogResult.OK)
            {
                inputFileValue.Text = selectInputFile.FileName;
                model.InputPath = inputFileValue.Text;

                outputFileValue.Text = model.InputPath.Replace(".nmea", "_new.nmea");
                model.OutputPath = outputFileValue.Text;
            }
        }

        private void SetOutput(string inputPath)
        {          
            OpenFileDialog selectOutputPath = new OpenFileDialog();
            selectOutputPath.Title = "Select file...";
            selectOutputPath.InitialDirectory = model.OutputPath;
            selectOutputPath.Filter = ".nmea (*.nmea)|*.nmea";
            selectOutputPath.FilterIndex = 1;
            selectOutputPath.RestoreDirectory = true;
            if (selectOutputPath.ShowDialog() == DialogResult.OK)
            {
                outputFileValue.Text = selectOutputPath.FileName;
                model.OutputPath = outputFileValue.Text;
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {           
            this.Enabled = false;
            ConvertProgresser.RunWorkerAsync();
        }

        private void Calc_ConvertProgressEvent(int ProgressValue)
        {
            ConvertProgresser.ReportProgress(ProgressValue);
        }

        private void ConvertProgresser_DoWork(object sender, DoWorkEventArgs e)
        {
            Calculation calc = new Calculation();
            calc.ConvertProgressEvent += Calc_ConvertProgressEvent;
            calc.ConvertInitializer(model);
            calc.Convert(model);
            calc.WriteResult(model);
        }

        private void ConvertProgresser_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Enabled = true;
        }

        private void ConvertProgresser_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }
    }
}