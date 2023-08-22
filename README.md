# Cars

## Table script

```
create database Cars
go

use Cars
go

create table car
(
    id         int identity,
    make       varchar(50),
    model      varchar(50),
    model_year int,
    price      decimal,
    deleted    int

    constraint car_pk
        primary key (id)
)
go
```