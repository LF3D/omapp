using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.Framework.Components
{
    /// <summary>
    /// Represents orbit parameters.
    /// </summary>
    public class OrbitParameter
    {
        /// <summary>
        /// Create a new instance of <see cref="OrbitParameter"/> class.
        /// </summary>
        public OrbitParameter() { }
        /// <summary>
        /// Gets/Sets the identification(Integer Type).
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Gets/Sets current value.
        /// </summary>
        public string Value { get; set; }
    }
}
