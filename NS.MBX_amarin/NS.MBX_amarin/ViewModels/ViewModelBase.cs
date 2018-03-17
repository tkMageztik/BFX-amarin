using Acr.UserDialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NS.MBX_amarin.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }
        protected IPageDialogService DialogService { get; private set; }
        protected IUserDialogs UserDialogs { get; private set; }
        protected NavigationParameters RefNavParameters { get; set; }

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

        public ViewModelBase(INavigationService navigationService, IUserDialogs userDialogs)
        {
            NavigationService = navigationService;
            UserDialogs = userDialogs;
        }

        public ViewModelBase(INavigationService navigationService, IPageDialogService pageDialogService, IUserDialogs userDialogs)
        {
            NavigationService = navigationService;
            DialogService = pageDialogService;
            UserDialogs = userDialogs;
        }

        //cuando se sale de esta pagina
        public virtual void OnNavigatedFrom(NavigationParameters parametros)
        {
            
        }

        //cuando se navega hacia aqui incluyendo backbutton
        public virtual void OnNavigatedTo(NavigationParameters parametros)
        {
            
        }

        //cuando se navega hacia aqui de ida, no de regreso con el back button
        public virtual void OnNavigatingTo(NavigationParameters parametros)
        {
            
        }

        public virtual void Destroy()
        {
            
        }

        //metodo que permite crear un nuevo navigation parameters sin tener que apuntar al mismo anterior
        //por defecto no incluye la pagina de origen, a menos que sea una pagina muy frecuentada
        protected NavigationParameters GetNavigationParameters()
        {
            return GetNavigationParameters(false, null);
        }

        protected NavigationParameters GetNavigationParameters(string listaParametrosNoIncluir)
        {
            return GetNavigationParameters(false, listaParametrosNoIncluir);
        }

        //si la lista tiene parametros, estos no se colocaran en el nuevo navigation parameters
        protected NavigationParameters GetNavigationParameters(bool incluirPageOrigen, string parametrosNoIncluir)
        {
            NavigationParameters navParameters = new NavigationParameters();

            if (RefNavParameters != null)
            {
                List<string> listaParametrosNoIncluye = null;
                if(parametrosNoIncluir != null)
                {
                    listaParametrosNoIncluye = parametrosNoIncluir.Split(',').ToList();
                }

                foreach (KeyValuePair<string, object> navigationParameter in RefNavParameters)
                {
                    //la pagina origen debe ser colocada en cada pagina individual
                    if (incluirPageOrigen == true || (incluirPageOrigen == false && navigationParameter.Key != Constantes.pageOrigen))
                    {
                        bool encontroEnLista = false;

                        if (listaParametrosNoIncluye != null)
                        {
                            foreach(string item in listaParametrosNoIncluye)
                            {
                                if(item == navigationParameter.Key)
                                {
                                    encontroEnLista = true;
                                    break;
                                }
                            }
                        }

                        if(!encontroEnLista)
                        {
                            navParameters.Add(navigationParameter.Key, navigationParameter.Value);
                        }
                    }
                }
            }

            return navParameters;
        }
    }
}
