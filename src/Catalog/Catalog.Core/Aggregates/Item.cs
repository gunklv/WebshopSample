using Catalog.Domain.Aggregates.Abstractions;
using Catalog.Domain.Exceptions;

namespace Catalog.Domain.Aggregates
{
    public class Item : IAggregateRoot
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

        private Category _category;
        public Category Category
        {
            get { return _category; }
            set
            {
                if (value == null)
                    throw new InvalidStateException($"'{nameof(Category)}' field can not be null.");

                _category = value;
            }
        }

        public Item(long? id, string name, string description, string imageUrl, decimal price, long amount, Category category)
        {
            Id = id.GetValueOrDefault();
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
            Amount = amount;
            Category = category;
        }
    }
}
