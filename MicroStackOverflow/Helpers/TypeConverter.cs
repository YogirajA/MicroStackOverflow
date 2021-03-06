﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;

namespace MicroStackOverflow.Helpers
{
   // http://stackoverflow.com/questions/7778216/automapper-or-similar-allow-mapping-of-dynamic-types
    public static class TypeConverter
    {
        public static T FromDynamicToStatic<T>(object expando)
        {
            var entity = Activator.CreateInstance<T>();

            //ExpandoObject implements dictionary
            var properties = expando as IDictionary<string, object>;

            if (properties == null)
                return entity;

            foreach (var entry in properties)
            {
                var propertyInfo = entity.GetType().GetProperty(entry.Key);
                if (propertyInfo != null)
                    propertyInfo.SetValue(entity, entry.Value, null);
            }
            return entity;
        }

        public static dynamic FromStaticToDynamic(object staticObject)
        {
            dynamic dynamicObject = new ExpandoObject();
           
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(staticObject))
            {
                ((IDictionary<string, object>)dynamicObject)
                    .Add(prop.Name, prop.GetValue(staticObject));
               
            }
            return dynamicObject;
        }
    }
}