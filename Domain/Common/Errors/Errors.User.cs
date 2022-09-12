using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubberDinner.Domain.Common.Errors
{
    public static partial class Errors
    {

        public static class User
        {
            public static Error DuplicateEmail => Error.Conflict(
                code: "User.DuplicateEmail",
                description: "Email Is already in use");

            public static Error DuplicateUsername => Error.Conflict( code:"User.DuplicateUsername",
                                                                        description:"Username ya existe");
        }
    }
}
