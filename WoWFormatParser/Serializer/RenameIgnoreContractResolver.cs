using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FileParser.FileTypes.Serializers
{
    public class RenameIgnoreContractResolver : DefaultContractResolver
    {
        internal bool IgnoreNullOrEmpty { get; set; } = true;

        private readonly HashSet<Type> _ignoredTypes;
        private readonly Dictionary<Type, HashSet<string>> _ignoredProperties;
        private readonly Dictionary<Type, Dictionary<string, string>> _renames;

        public RenameIgnoreContractResolver()
        {
            _ignoredTypes = new HashSet<Type>();
            _ignoredProperties = new Dictionary<Type, HashSet<string>>();
            _renames = new Dictionary<Type, Dictionary<string, string>>();
        }

        /// <summary>
        /// Excludes the specified type from being serialized.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public RenameIgnoreContractResolver IgnoreType(params Type[] types)
        {
            _ignoredTypes.UnionWith(types);
            return this;
        }
        /// <summary>
        /// Excludes the specified property from being serialized.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="jsonPropertyNames"></param>
        /// <returns></returns>
        public RenameIgnoreContractResolver IgnoreProperty(Type type, params string[] jsonPropertyNames)
        {
            if (!_ignoredProperties.ContainsKey(type))
                _ignoredProperties[type] = new HashSet<string>();

            foreach (var prop in jsonPropertyNames)
                _ignoredProperties[type].Add(prop);

            return this;
        }
        /// <summary>
        /// Renames the specified property when serialized.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <param name="newJsonPropertyName"></param>
        /// <returns></returns>
        public RenameIgnoreContractResolver RenameProperty(Type type, string propertyName, string newJsonPropertyName)
        {
            if (!_renames.ContainsKey(type))
                _renames[type] = new Dictionary<string, string>();

            _renames[type][propertyName] = newJsonPropertyName;
            return this;
        }


        /// <summary>
        /// Removes the specified type from the serializer's exclusion list.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public RenameIgnoreContractResolver RemoveTypeIgnore(params Type[] types)
        {
            foreach (var type in types)
                _ignoredTypes.Remove(type);

            return this;
        }
        /// <summary>
        /// Removes the specified property from the serializer's exclusion list.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="jsonPropertyNames"></param>
        /// <returns></returns>
        public RenameIgnoreContractResolver RemoveIgnoreProperty(Type type, params string[] jsonPropertyNames)
        {
            if (!_ignoredProperties.ContainsKey(type))
                return this;

            foreach (var prop in jsonPropertyNames)
                _ignoredProperties[type].Remove(prop);

            return this;
        }
        /// <summary>
        /// Removes the speficied property rename.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public RenameIgnoreContractResolver RemovePropertyRename(Type type, string propertyName)
        {
            if (!_renames.ContainsKey(type))
                return this;

            _renames[type].Remove(propertyName);
            return this;
        }


        public bool IsIgnored(Type type)
        {
            return _ignoredTypes.Contains(type);
        }

        public bool IsIgnored(Type type, string jsonPropertyName)
        {
            if (!_ignoredProperties.ContainsKey(type))
                return false;

            return _ignoredProperties[type].Contains(jsonPropertyName);
        }

        public bool IsRenamed(Type type, string jsonPropertyName, out string newJsonPropertyName)
        {
            if (_renames.TryGetValue(type, out Dictionary<string, string> renames))
            {
                if (renames.TryGetValue(jsonPropertyName, out newJsonPropertyName))
                    return true;
            }

            newJsonPropertyName = jsonPropertyName;
            return false;
        }


        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            ApplyConditions(property);

            if (IsRenamed(property.DeclaringType, property.PropertyName, out var newJsonPropertyName))
                property.PropertyName = newJsonPropertyName;

            return property;
        }

        private void ApplyConditions(JsonProperty property)
        {
            if (IsIgnored(property.DeclaringType) || IsIgnored(property.DeclaringType, property.PropertyName))
            {
                property.ShouldSerialize = i => false;
                property.Ignored = true;
                return;
            }

            if (IgnoreNullOrEmpty)
            {
                if (property.PropertyType == typeof(string))
                {
                    // ignore strings that are null or empty
                    property.ShouldSerialize = instance => !string.IsNullOrEmpty(GetValue<string>(instance, property.UnderlyingName));
                }
                else if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    // ignore empty collections
                    property.ShouldSerialize = instance => GetValue<IEnumerable>(instance, property.UnderlyingName)?.Cast<object>().Count() > 0;
                }
            }
        }

        private T GetValue<T>(object instance, string name) where T : class
        {
            if (instance == null)
                return null;

            Type type = instance.GetType();
            return (type.GetProperty(name) ?? (dynamic)type.GetField(name))?.GetValue(instance) as T;
        }
    }
}
