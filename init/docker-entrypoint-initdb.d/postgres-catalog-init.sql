CREATE TABLE IF NOT EXISTS category
        (
        id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
        parentId uuid,
        name text,
        imageUrl text
        );

CREATE TABLE IF NOT EXISTS item
        (
        id bigserial PRIMARY KEY,
        categoryId uuid,
        name text,
        description text,
        imageUrl text,
        price decimal,
        amount bigint
        );

CREATE TABLE IF NOT EXISTS integrationEventOutbox
        (
        eventId uuid PRIMARY KEY DEFAULT gen_random_uuid(),
        eventType text,
        createdOn timestamp,
        processedOn timestamp,
        payload text
        );