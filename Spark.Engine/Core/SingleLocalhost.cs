﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Core
{
    public class Localhost : ILocalhost
    {
        public Uri Base { get; set; }

        public Localhost(Uri baseuri)
        {
            this.Base = baseuri;
        }

        public Uri Absolute(Uri uri)
        {
            return uri.IsAbsoluteUri ? uri : new Uri(Base, uri.ToString());
        }

        public bool IsBaseOf(Uri uri)
        {
            if (uri.IsAbsoluteUri)
            {
                bool isbase = Base.Bugfixed_IsBaseOf(uri);
                return isbase;
            }
            else
            {
                return false;
            }
            
        }

        public Uri GetBaseOf(Uri uri)
        {
            return (this.IsBaseOf(uri)) ? this.Base : null;
        }
    }

    
}