using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmSchemats
{
    public class ViewModel : ObservableObject, IDataErrorInfo
    {
        public string this[string columnName]
        {
            get { return OnValidate(columnName); }
        }

        public string Error { get; private set;

            //get { throw new NotSupportedException(); }
        }

        protected virtual string OnValidate(string propertyName)
        {
            var context = new ValidationContext(this)
            {
                MemberName = propertyName
            };
            var results = new Collection<ValidationResult>();
            bool isValid = Validator.TryValidateObject(this, context, results, true);

            if (!isValid)
            {
                ValidationResult result = results.SingleOrDefault(p =>
                                                                  p.MemberNames.Any(memberName =>
                                                                                    memberName == propertyName));
                return result == null ? null : result.ErrorMessage;
            }
            return null;
        }
    }

}
