//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

using System.Linq;

namespace EyeXFramework
{
    using System.Collections.Generic;
    using Tobii.EyeX.Client;

    /// <summary>
    /// Accesses and monitors enumerable engine states.
    /// Used by the EyeXHost.
    /// </summary>
    /// <typeparam name="T">Data type of the engine state.</typeparam>
    internal class EnumerableStateAccessor<T> : EngineStateAccessor<T[]>
    {
        public EnumerableStateAccessor(string statePath)
            : base(statePath)
        {
        }

        /// <summary>
        /// Gets the data from the state bag.
        /// </summary>
        /// <param name="bag">The bag.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if data could be retrieved; otherwise <c>false</c>.</returns>
        protected override bool GetData(StateBag bag, out T[] value)
        {
            IEnumerable<T> enumerableValue;
            var result = bag.TryGetStateValueAsEnumerable(out enumerableValue, StatePath);
            value = result ? enumerableValue.ToArray() : default(T[]);
            return result;
        }
    }
}