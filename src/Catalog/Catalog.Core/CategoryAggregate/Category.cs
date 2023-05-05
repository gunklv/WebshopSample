using Catalog.Domain.CategoryAggregate.Events;
using Catalog.Domain.Shared.Aggregates;
using Catalog.Domain.Shared.Exceptions;

namespace Catalog.Domain.CategoryAggregate
{
    public class Category : AggregateRoot
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

        public void UpdateProperties(
            Category parentCategory,
            string name,
            string imageUrl)
        {
            ParentCategory = parentCategory;
            Name = name;
            ImageUrl = imageUrl;
        }

        public void MarkDeleted()
        {
            AddDomainEvent(new CategoryDeletedEvent(Id));
        }
    }
}
