"""
Bridge Pattern (Мост)

Отделяет абстракцию от её реализации так, чтобы и то, и другое можно было изменять независимо.
Позволяет избежать зависимости между абстракцией и реализацией.
"""


class Color:
    """Интерфейс реализации - Цвет"""
    
    def apply_color(self) -> str:
        pass


class RedColor(Color):
    """Конкретная реализация - Красный цвет"""
    
    def apply_color(self) -> str:
        return "красный"


class BlueColor(Color):
    """Конкретная реализация - Синий цвет"""
    
    def apply_color(self) -> str:
        return "синий"


class Shape:
    """Абстракция - Фигура"""
    
    def __init__(self, color: Color):
        self.color = color
    
    def draw(self) -> str:
        pass


class Circle(Shape):
    """Уточненная абстракция - Круг"""
    
    def draw(self) -> str:
        return f"Рисую круг {self.color.apply_color()} цвета"


class Square(Shape):
    """Уточненная абстракция - Квадрат"""
    
    def draw(self) -> str:
        return f"Рисую квадрат {self.color.apply_color()} цвета"


def demo():
    """Демонстрация работы паттерна Bridge"""
    print("=== Паттерн Bridge (Мост) ===\n")
    
    # Создаем разные комбинации фигур и цветов
    red_circle = Circle(RedColor())
    print(red_circle.draw())
    
    blue_circle = Circle(BlueColor())
    print(blue_circle.draw())
    
    red_square = Square(RedColor())
    print(red_square.draw())
    
    blue_square = Square(BlueColor())
    print(blue_square.draw())


if __name__ == "__main__":
    demo()
