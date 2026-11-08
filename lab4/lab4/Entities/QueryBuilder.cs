namespace lab4.Entities;

using System;
using System.Linq.Expressions;

public static class QueryBuilder
{
    public static Expression<Func<Car, bool>> Build(FilterData filter)
    {
        var carParam = Expression.Parameter(typeof(Car), "car");

        Expression? body = null;
        
        void AddCondition(Expression condition)
        {
            body = body == null ? condition : Expression.AndAlso(body, condition);
        }
        
        if (FilterData.IsSet(filter.MinYear))
        {
            AddCondition(
                Expression.GreaterThanOrEqual(
                    Expression.Property(carParam, nameof(Car.Year)),
                    Expression.Constant(filter.MinYear!.Value, typeof(int))
                )    
            );
        }
        
        if (FilterData.IsSet(filter.MaxYear))
        {
            AddCondition(
                Expression.LessThanOrEqual(
                    Expression.Property(carParam, nameof(Car.Year)),
                    Expression.Constant(filter.MaxYear!.Value, typeof(int))
                )
            );
        }

        // car.Mileage <= filter.MaxMileage
        if (FilterData.IsSet(filter.MaxMileage))
        {
            AddCondition(
                Expression.LessThanOrEqual(
                    Expression.Property(carParam, nameof(Car.Mileage)),
                    Expression.Constant(filter.MaxMileage.Value, typeof(int))
                )
            );
        }
        
        if (FilterData.IsSet(filter.MinPower))
        {
            AddCondition(
                Expression.GreaterThanOrEqual(
                    Expression.Property(carParam, nameof(Car.Power)),
                    Expression.Constant(filter.MinPower.Value, typeof(int))
                )
            );
        }
        
        if (filter.AutoGearBox.HasValue)
        {
            AddCondition(
                Expression.Equal(
                    Expression.Property(carParam, nameof(Car.AutoGearBox)),
                    Expression.Constant(filter.AutoGearBox.Value, typeof(bool))
                )
            );
        }
        
        if (body == null)
            body = Expression.Constant(true);
        
        return Expression.Lambda<Func<Car, bool>>(body, carParam);
    }
}