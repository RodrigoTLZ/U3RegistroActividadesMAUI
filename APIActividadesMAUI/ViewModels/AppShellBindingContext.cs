using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIActividadesMAUI.ViewModels
{
    public class AppShellBindingContext
    {
        public ActividadesViewModel ActividadesViewModel { get; set; } = new ActividadesViewModel();
        public DepartamentosViewModel DepartamentosViewModel { get; set; } = new DepartamentosViewModel();
        public LoginViewModel LoginViewModel { get; set; } = new();
    }
}
