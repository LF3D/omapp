using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.Framework.Components
{
    /// <summary>
    /// Represents plan parameters.
    /// </summary>
    public class PlanParameter
    {
        /// <summary>
        /// Create a new instance of <see cref="PlanParameter"/> class.
        /// </summary>
        public PlanParameter() { }
        /// <summary>
        /// Gets/Sets the identification(Integer Type).
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Gets/Sets current value.
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Gets/Sets current Hex Value.
        /// </summary>
        public string Hex { get; set; }
    }
}
