using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spotifyWPF.Model
{
    public class NavModel
    {

    }
    public class NavButton
    {
        public string Name { get; set; }
        public bool Active { get; set; }

        public NavButton(string name)
        {
            this.Name = name;
        }
    }
}
