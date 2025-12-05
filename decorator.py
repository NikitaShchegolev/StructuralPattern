"""
Decorator Pattern (Декоратор)

Динамически добавляет объектам новые обязанности.
Является гибкой альтернативой порождению подклассов для расширения функциональности.
"""

from abc import ABC, abstractmethod


class Coffee(ABC):
    """Базовый компонент - Кофе"""
    
    @abstractmethod
    def get_description(self) -> str:
        pass
    
    @abstractmethod
    def get_cost(self) -> float:
        pass


class SimpleCoffee(Coffee):
    """Конкретный компонент - Простой кофе"""
    
    def get_description(self) -> str:
        return "Простой кофе"
    
    def get_cost(self) -> float:
        return 50.0


class CoffeeDecorator(Coffee):
    """Базовый декоратор"""
    
    def __init__(self, coffee: Coffee):
        self._coffee = coffee
    
    def get_description(self) -> str:
        return self._coffee.get_description()
    
    def get_cost(self) -> float:
        return self._coffee.get_cost()


class MilkDecorator(CoffeeDecorator):
    """Конкретный декоратор - Молоко"""
    
    def get_description(self) -> str:
        return f"{self._coffee.get_description()} + молоко"
    
    def get_cost(self) -> float:
        return self._coffee.get_cost() + 20.0


class SugarDecorator(CoffeeDecorator):
    """Конкретный декоратор - Сахар"""
    
    def get_description(self) -> str:
        return f"{self._coffee.get_description()} + сахар"
    
    def get_cost(self) -> float:
        return self._coffee.get_cost() + 5.0


class WhippedCreamDecorator(CoffeeDecorator):
    """Конкретный декоратор - Взбитые сливки"""
    
    def get_description(self) -> str:
        return f"{self._coffee.get_description()} + взбитые сливки"
    
    def get_cost(self) -> float:
        return self._coffee.get_cost() + 30.0


def demo():
    """Демонстрация работы паттерна Decorator"""
    print("=== Паттерн Decorator (Декоратор) ===\n")
    
    # Простой кофе
    coffee = SimpleCoffee()
    print(f"{coffee.get_description()}: {coffee.get_cost()} руб.")
    
    # Кофе с молоком
    coffee_with_milk = MilkDecorator(SimpleCoffee())
    print(f"{coffee_with_milk.get_description()}: {coffee_with_milk.get_cost()} руб.")
    
    # Кофе с молоком и сахаром
    coffee_with_milk_and_sugar = SugarDecorator(MilkDecorator(SimpleCoffee()))
    print(f"{coffee_with_milk_and_sugar.get_description()}: {coffee_with_milk_and_sugar.get_cost()} руб.")
    
    # Полный набор
    fancy_coffee = WhippedCreamDecorator(SugarDecorator(MilkDecorator(SimpleCoffee())))
    print(f"{fancy_coffee.get_description()}: {fancy_coffee.get_cost()} руб.")


if __name__ == "__main__":
    demo()
