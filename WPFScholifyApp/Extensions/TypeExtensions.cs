// <copyright file="TypeExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.Extensions
{
    using System;

    public static class TypeExtensions
    {
        public static bool IsNullableReferenceType(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }
    }
}
