"""
Composite Pattern (Компоновщик)

Объединяет объекты в древовидную структуру для представления иерархии.
Позволяет клиентам единообразно трактовать индивидуальные и составные объекты.
"""

from abc import ABC, abstractmethod
from typing import List


class FileSystemComponent(ABC):
    """Компонент файловой системы"""
    
    @abstractmethod
    def show_details(self, indent: int = 0) -> str:
        pass
    
    @abstractmethod
    def get_size(self) -> int:
        pass


class File(FileSystemComponent):
    """Лист - Файл"""
    
    def __init__(self, name: str, size: int):
        self.name = name
        self.size = size
    
    def show_details(self, indent: int = 0) -> str:
        return f"{'  ' * indent}📄 Файл: {self.name} ({self.size} KB)"
    
    def get_size(self) -> int:
        return self.size


class Directory(FileSystemComponent):
    """Контейнер - Директория"""
    
    def __init__(self, name: str):
        self.name = name
        self.children: List[FileSystemComponent] = []
    
    def add(self, component: FileSystemComponent):
        self.children.append(component)
    
    def remove(self, component: FileSystemComponent):
        self.children.remove(component)
    
    def show_details(self, indent: int = 0) -> str:
        result = f"{'  ' * indent}📁 Папка: {self.name} ({self.get_size()} KB)\n"
        for child in self.children:
            result += child.show_details(indent + 1) + "\n"
        return result.rstrip()
    
    def get_size(self) -> int:
        return sum(child.get_size() for child in self.children)


def demo():
    """Демонстрация работы паттерна Composite"""
    print("=== Паттерн Composite (Компоновщик) ===\n")
    
    # Создаем структуру файловой системы
    root = Directory("root")
    
    documents = Directory("documents")
    documents.add(File("resume.pdf", 150))
    documents.add(File("letter.docx", 80))
    
    pictures = Directory("pictures")
    pictures.add(File("photo1.jpg", 2500))
    pictures.add(File("photo2.jpg", 3000))
    
    root.add(documents)
    root.add(pictures)
    root.add(File("readme.txt", 10))
    
    # Показываем структуру
    print(root.show_details())


if __name__ == "__main__":
    demo()
