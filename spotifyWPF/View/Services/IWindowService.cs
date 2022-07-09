using System;

namespace spotifyWPF.View.Services;

public interface IWindowService
{
   void ShowWindow(object viewmodel);
   void CloseWindowByType(Type t);
}