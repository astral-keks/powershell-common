using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AstralKeks.PowerShell.Common.Parameters
{
    internal class ParameterBinding
    {
        private readonly ParameterCompleterSource _source;
        private readonly List<Func<Type, object, object>> _converters;

        public ParameterBinding(ParameterCompleterSource source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _converters = new List<Func<Type, object, object>>
            {
                GetConvertibleValue,
                GetEnumerationValue,
                GetCollectionValue,
                GetSerializedValue,
                GetSingleValue,
                GetRawValue
            };
        }

        public void BindParameters(IDictionary parameters)
        {
            foreach (var property in _source.Properties)
            {
                var parameterName = property.Name;
                if (parameters.Contains(parameterName) && property.CanWrite)
                {
                    var parameterValue = parameters[parameterName];
                    ConvertParameterValue(property.PropertyType, parameterValue, 
                        v => property.SetValue(_source.Cmdlet, v));
                }
            }
            
            if (_source.DynamicDictionary != null)
            {
                foreach (var runtimeDefinedParameter in _source.DynamicDictionary.Values)
                {
                    var parameterName = runtimeDefinedParameter.Name;
                    if (parameters.Contains(parameterName))
                    {
                        var parameterValue = parameters[parameterName];
                        ConvertParameterValue(runtimeDefinedParameter.ParameterType, parameterValue,
                            v => runtimeDefinedParameter.Value = v);
                    }
                }
            }

            if (_source.DynamicProperties != null)
            {
                foreach (var property in _source.DynamicProperties)
                {
                    var parameterName = property.Name;
                    if (parameters.Contains(parameterName) && property.CanWrite)
                    {
                        var parameterValue = parameters[parameterName];
                        ConvertParameterValue(property.PropertyType, parameterValue,
                            v => property.SetValue(_source.DynamicParameters, v));
                    }
                }
            }
        }

        private object ConvertParameterValue(Type parameterType, object parameterValue, Action<object> onConverted = null)
        {
            object result = null;

            foreach (var converter in _converters)
            {
                try
                {
                    result = converter(parameterType, parameterValue);
                    onConverted?.Invoke(result);
                    break;
                }
                catch (Exception)
                {
                    result = null;
                }
            }

            return result;
        }
        
        private object GetConvertibleValue(Type parameterType, object parameterValue)
        {
            return Convert.ChangeType(parameterValue, parameterType);
        }

        private object GetEnumerationValue(Type parameterType, object parameterValue)
        {
            return Enum.Parse(parameterType, (string)parameterValue);
        }

        private object GetCollectionValue(Type parameterType, object parameterValue)
        {
            var intermediateArray = (parameterValue as IEnumerable).Cast<object>().ToArray();

            var sourceItemType = intermediateArray.Any() ? intermediateArray.First().GetType() : typeof(object);
            var targetItemType = parameterType.IsGenericType ? parameterType.GetGenericArguments().First() : typeof(object);
            var sourceArray = intermediateArray.Select(o => ConvertParameterValue(targetItemType, o)).ToArray();
            var targetArray = Array.CreateInstance(targetItemType, sourceArray.Length);
            Array.Copy(sourceArray, targetArray, sourceArray.Length);
            parameterValue = targetArray;

            return GetSerializedValue(parameterType, parameterValue);
        }

        private object GetSerializedValue(Type parameterType, object parameterValue)
        {
            var content = new StringBuilder();

            var writer = new StringWriter(content);
            var serializer = new XmlSerializer(parameterValue.GetType());
            serializer.Serialize(writer, parameterValue);

            var reader = new StringReader(content.ToString());
            var deserializer = new XmlSerializer(parameterType);
            var obj = deserializer.Deserialize(reader);

            return obj;
        }

        private object GetSingleValue(Type parameterType, object parameterValue)
        {
            return GetCollectionValue(parameterType, new[] { parameterValue });
        }

        private object GetRawValue(Type parameterType, object parameterValue)
        {
            return parameterValue;
        }
    }
}
