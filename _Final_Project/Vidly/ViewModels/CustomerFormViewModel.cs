using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vidly.Models;

namespace Vidly.ViewModels
{
    public class CustomerFormViewModel
    {
        public IEnumerable<MembershipType> MembershipTypes { get; set; } // We use IEnumberable<> here instead of List<> because we will not need to use the addtional functionality a list provides since users will never be adding/removing/updating membership types
        public Customer Customer { get; set; }
    }
}