﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuGetGallery.Data
{
    public interface IDbModelManager
    {
        DbCompiledModel GetCurrentModel();
        void ReplaceCurrentModel(DbCompiledModel model);
        void RebuildModel();
    }
}
