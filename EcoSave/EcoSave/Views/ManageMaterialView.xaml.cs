﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EcoSave.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageMaterialView : ContentPage
    {
        public ManageMaterialView()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //materialListView.ItemSource = await MaterialDA.GetAllMaterials();
        }
    }
}