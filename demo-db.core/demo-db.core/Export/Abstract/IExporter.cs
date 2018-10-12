using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.core.Export.Abstract
{
    public interface IExporter
    {
        void GenerateReport(IList<Services.ViewModels.GradeViewModel> grades, string username);
    }
}
