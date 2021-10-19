using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Reflection;

namespace CRUD.Helper
{
  public class ViewModelBase : NotifyPropertyChanges
    {


        public ViewModelBase()
        {
        }

        public ViewModelBase(Window window)
        {
            _window = window;
        }

        private Window _window;



    

        public void ClearComboBox(object param)
        {
            var combo = param as ComboBox;
            if (combo != null) { combo.SelectedIndex = 0; combo.IsDropDownOpen = false; }
        }


        public static void SetPropNested(object view, string name, object value)
        {
            Type viewType = view.GetType();
            var list = name.Split('.');
            var type1 = viewType.GetProperty(list[0]);

        }

        public static Object GetPropValue(Object obj, String name)
        {
            foreach (String part in name.Split('.'))
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }

        public static T GetPropValue<T>(Object obj, String name)
        {
            Object retval = GetPropValue(obj, name);
            if (retval == null) { return default(T); }

            // throws InvalidCastException if types are incompatible
            return (T)retval;
        }

        public void WindowLoaded(object sender, EventArgs e)
        {
            Window window = sender as Window;
            if (window != null)
                _window = window;
        }

        public void UserControlLoaded(object sender, EventArgs e)
        {
            UserControl userControl = sender as UserControl;
            if (userControl != null)
            {
                Window window = new Window
                {
                    Content = userControl
                };

                _window = window;
            }
        }

      

        private Dictionary<string, string> _errorData;
        public Dictionary<string, string> ErrorData
        {
            get
            {
                if (_errorData == null)
                    _errorData = CreateErrorData();
                return _errorData;
            }
            set { _errorData = value; }
        }

        
        public virtual Dictionary<string, string> CreateErrorData()
        {
            return null;
        }
    }
}
