using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NS.MBX_amarin.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }
        protected IPageDialogService DialogService { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public ViewModelBase(INavigationService navigationService, IPageDialogService pageDialogService)
        {
            NavigationService = navigationService;
            DialogService = pageDialogService;
        }

        public virtual void OnNavigatedFrom(NavigationParameters parametros)
        {
            
        }

        //cuando se navega hacia aqui
        public virtual void OnNavigatedTo(NavigationParameters parametros)
        {
            
        }

        public virtual void OnNavigatingTo(NavigationParameters parametros)
        {
            
        }

        public virtual void Destroy()
        {
            
        }
    }
}
