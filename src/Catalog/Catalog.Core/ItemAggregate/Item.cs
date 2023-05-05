using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.ItemAggregate.Events;
using Catalog.Domain.Shared.Aggregates;
using Catalog.Domain.Shared.Exceptions;

namespace Catalog.Domain.ItemAggregate
{
    public class Item : AggregateRoot
    {
        public long Id { get; private set; }

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

        public string Description { get; set; }
        public string ImageUrl { get; set; }

        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set
            {
                if (value < 0)
                    throw new InvalidStateException($"'{nameof(Price)}' field can not be negative.");

                _price = value;
            }
        }

        private long _amount;
        public long Amount
        {
            get { return _amount; }
            set
            {
                if (value < 0)
                    throw new InvalidStateException($"'{nameof(Amount)}' field can not be negative.");

                _amount = value;
            }
        }

        private Guid _categoryId;
        public Guid CategoryId
        {
            get { return _categoryId; }
            set
            {
                if (value == default)
                    throw new InvalidStateException($"'{nameof(Category)}' field can not be default.");

                _categoryId = value;
            }
        }

        public Item(long? id, string name, string description, string imageUrl, decimal price, long amount, Guid categoryId)
        {
            Id = id.GetValueOrDefault();
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
            Amount = amount;
            CategoryId = categoryId;
        }

        public void UpdateProperties(
            string name,
            string description,
            string imageUrl,
            decimal price,
            long amount,
            Guid categoryId)
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
            Amount = amount;
            CategoryId = categoryId;

            AddDomainEvent(new ItemPropertiesUpdatedEvent(this));
        }
    }
}
