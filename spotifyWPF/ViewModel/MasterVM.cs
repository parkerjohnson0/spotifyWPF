using spotifyWPF.Model.Nav;

namespace spotifyWPF.ViewModel;

public static class MasterVM
{
    public static NavVM NavVM { get; set; }
    public static AuthorizeVM AuthorizeVM { get; set; }
    public static PlayerVM PlayerVM { get; set; }
    public static RootVM RootVM { get; set; }
     static MasterVM()
     {
         NavVM = new NavVM();
         AuthorizeVM = new AuthorizeVM();
         PlayerVM = new PlayerVM();
         RootVM = new RootVM();
     }
}