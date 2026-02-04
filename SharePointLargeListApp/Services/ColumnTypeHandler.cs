using Microsoft.SharePoint.Client;

namespace SharePointLargeListApp.Services
{
    public static class ColumnTypeHandler
    {
        /// <summary>
        /// Converts calculated column value to appropriate type for target column
        /// </summary>
        public static object? ConvertColumnValue(object? calculatedValue, FieldType targetFieldType)
        {
            if (calculatedValue == null)
                return null;

            try
            {
                switch (targetFieldType)
                {
                    case FieldType.Text:
                    case FieldType.Note:
                        return ConvertToText(calculatedValue);

                    case FieldType.Number:
                    case FieldType.Currency:
                        return ConvertToNumber(calculatedValue);

                    case FieldType.DateTime:
                        return ConvertToDateTime(calculatedValue);

                    case FieldType.Boolean:
                        return ConvertToBoolean(calculatedValue);

                    case FieldType.Integer:
                        return ConvertToInteger(calculatedValue);

                    case FieldType.User:
                    case FieldType.Lookup:
                        return ConvertToLookup(calculatedValue);

                    case FieldType.URL:
                        return ConvertToUrl(calculatedValue);

                    case FieldType.Choice:
                        return ConvertToText(calculatedValue);

                    default:
                        return calculatedValue?.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error converting value '{calculatedValue}' to type {targetFieldType}: {ex.Message}", ex);
            }
        }

        private static string ConvertToText(object value)
        {
            if (value is FieldLookupValue lookup)
                return lookup.LookupValue;

            if (value is FieldUserValue user)
                return user.LookupValue;

            return value?.ToString() ?? string.Empty;
        }

        private static double ConvertToNumber(object value)
        {
            var strValue = value?.ToString()?.Replace("$", "").Replace(",", "").Trim() ?? "0";
            return double.TryParse(strValue, out var result) ? result : 0;
        }

        private static int ConvertToInteger(object value)
        {
            var strValue = value?.ToString()?.Trim() ?? "0";
            return int.TryParse(strValue, out var result) ? result : 0;
        }

        private static DateTime? ConvertToDateTime(object value)
        {
            if (value is DateTime dt)
                return dt;

            var strValue = value?.ToString()?.Trim();
            if (string.IsNullOrEmpty(strValue))
                return null;

            return DateTime.TryParse(strValue, out var result) ? result : null;
        }

        private static bool ConvertToBoolean(object value)
        {
            if (value is bool b)
                return b;

            var strValue = value?.ToString()?.ToLower().Trim();
            return strValue == "true" || strValue == "yes" || strValue == "1";
        }

        private static FieldLookupValue? ConvertToLookup(object value)
        {
            if (value is FieldLookupValue lookup)
                return lookup;

            if (value is FieldUserValue user)
                return new FieldLookupValue { LookupId = user.LookupId };

            // Try to parse "ID;#Value" format
            var strValue = value?.ToString();
            if (!string.IsNullOrEmpty(strValue) && strValue.Contains(";#"))
            {
                var parts = strValue.Split(new[] { ";#" }, StringSplitOptions.None);
                if (parts.Length >= 2 && int.TryParse(parts[0], out var id))
                {
                    return new FieldLookupValue { LookupId = id };
                }
            }

            return null;
        }

        private static FieldUrlValue? ConvertToUrl(object value)
        {
            if (value is FieldUrlValue urlValue)
                return urlValue;

            var strValue = value?.ToString();
            if (!string.IsNullOrEmpty(strValue))
            {
                // Check if it's in "URL, Description" format
                if (strValue.Contains(","))
                {
                    var parts = strValue.Split(',');
                    return new FieldUrlValue
                    {
                        Url = parts[0].Trim(),
                        Description = parts.Length > 1 ? parts[1].Trim() : parts[0].Trim()
                    };
                }

                return new FieldUrlValue
                {
                    Url = strValue,
                    Description = strValue
                };
            }

            return null;
        }

        /// <summary>
        /// Gets the field type for a column
        /// </summary>
        public static FieldType GetFieldType(ClientContext context, string listName, string fieldName)
        {
            var list = context.Web.Lists.GetByTitle(listName);
            var field = list.Fields.GetByInternalNameOrTitle(fieldName);
            context.Load(field, f => f.FieldTypeKind);
            context.ExecuteQuery();
            return field.FieldTypeKind;
        }

        /// <summary>
        /// Validates that the calculated column value can be converted to target column type
        /// </summary>
        public static bool ValidateConversion(object? calculatedValue, FieldType targetFieldType, out string? errorMessage)
        {
            errorMessage = null;
            try
            {
                ConvertColumnValue(calculatedValue, targetFieldType);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}
