using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GhostCore.Foundation
{
    public class ValidationCollection : List<ValidationMessage>
    {
        /// <summary>
        /// An empty <see cref="ValidationCollection"/>.
        /// </summary>
        public static readonly ValidationCollection Empty = new ValidationCollection();

        /// <summary>
        /// Searches through all the items to determine if this collection has at least one error.
        /// </summary>
        public bool IsError => this.Any(x => x.IsError);

        /// <summary>
        /// Aggregate messages from all the <see cref="ValidationMessage"/> objects contained in the collection.
        /// Each message is on a new line.
        /// </summary>
        public string AllMessages
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                foreach (var msgs in this)
                {
                    sb.AppendLine($"{msgs.Prefix}{msgs.Message}");
                }

                return sb.ToString();
            }
        }

        public void RemoveNonErrors()
        {
            var src = this.ToList();
            foreach (var item in src)
            {
                if (!item.IsError)
                    Remove(item);
            }
        }
    }
}
