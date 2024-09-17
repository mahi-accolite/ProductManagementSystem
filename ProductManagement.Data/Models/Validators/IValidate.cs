using ProductManagement.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Data.Models.Validators
{
    public interface IValidateProduct
    {
        void Check(ProductDto product);
    }

}
