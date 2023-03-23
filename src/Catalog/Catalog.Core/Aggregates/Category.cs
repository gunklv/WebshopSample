using Catalog.Domain.Aggregates.Abstractions;
using Catalog.Domain.Exceptions;

namespace Catalog.Domain.Aggregates
{
    public class Category : IAggregateRoot
    {
        public Guid Id { get; private set; }
        public Category ParentCategory { get; set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new InvalidStateException($"'{nameof(Name)}' field can not be null or empty.");

                if (value.Length > 50)
                    throw new InvalidStateException($"'{nameof(Name)}' field's length can not be longer than 50 characters.");

                _name = value;
            }
        }

        public string ImageUrl { get; set; }

        public Category(Guid? id, string name, string imageUrl, Category parentCategory)
        {
            Id = id.GetValueOrDefault();
            Name = name;
            ImageUrl = imageUrl;
            ParentCategory = parentCategory;
        }
    }
}
