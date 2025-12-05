"""
Proxy Pattern (Заместитель)

Предоставляет суррогат или заместитель другого объекта для контроля доступа к нему.
Может добавлять дополнительную функциональность при обращении к объекту.
"""

from abc import ABC, abstractmethod


class Image(ABC):
    """Интерфейс изображения"""
    
    @abstractmethod
    def display(self):
        pass


class RealImage(Image):
    """Реальное изображение"""
    
    def __init__(self, filename: str):
        self.filename = filename
        self._load_from_disk()
    
    def _load_from_disk(self):
        print(f"Загрузка изображения с диска: {self.filename}")
    
    def display(self):
        print(f"Отображение изображения: {self.filename}")


class ProxyImage(Image):
    """Прокси для ленивой загрузки изображения"""
    
    def __init__(self, filename: str):
        self.filename = filename
        self._real_image = None
    
    def display(self):
        if self._real_image is None:
            # Ленивая загрузка - создаем реальный объект только при первом обращении
            self._real_image = RealImage(self.filename)
        self._real_image.display()


class ProtectedProxy(Image):
    """Прокси с контролем доступа"""
    
    def __init__(self, filename: str, user_role: str):
        self.filename = filename
        self.user_role = user_role
        self._real_image = None
    
    def display(self):
        if self.user_role != "admin":
            print(f"Доступ запрещен! Изображение {self.filename} доступно только для администраторов.")
            return
        
        if self._real_image is None:
            self._real_image = RealImage(self.filename)
        self._real_image.display()


def demo():
    """Демонстрация работы паттерна Proxy"""
    print("=== Паттерн Proxy (Заместитель) ===\n")
    
    print("1. Виртуальный прокси (ленивая загрузка):")
    print("Создание прокси (изображение еще не загружено):")
    image1 = ProxyImage("photo1.jpg")
    print("\nПервый вызов display() - загрузка происходит:")
    image1.display()
    print("\nВторой вызов display() - загрузка не нужна:")
    image1.display()
    
    print("\n" + "="*50 + "\n")
    
    print("2. Защищающий прокси (контроль доступа):")
    print("Пользователь с ролью 'user':")
    image2 = ProtectedProxy("secret.jpg", "user")
    image2.display()
    
    print("\nПользователь с ролью 'admin':")
    image3 = ProtectedProxy("secret.jpg", "admin")
    image3.display()


if __name__ == "__main__":
    demo()
