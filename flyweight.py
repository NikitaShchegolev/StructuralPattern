"""
Flyweight Pattern (Приспособленец/Легковес)

Использует разделение для эффективной поддержки множества мелких объектов.
Экономит память за счет разделения состояния между похожими объектами.
"""

from typing import Dict


class TreeType:
    """Приспособленец - разделяемое состояние дерева"""
    
    def __init__(self, name: str, color: str, texture: str):
        self.name = name
        self.color = color
        self.texture = texture
    
    def draw(self, x: int, y: int):
        print(f"Рисую {self.name} дерево ({self.color}, {self.texture}) в позиции ({x}, {y})")


class TreeFactory:
    """Фабрика приспособленцев"""
    
    _tree_types: Dict[str, TreeType] = {}
    
    @classmethod
    def get_tree_type(cls, name: str, color: str, texture: str) -> TreeType:
        """Возвращает существующий или создает новый тип дерева"""
        key = f"{name}_{color}_{texture}"
        
        if key not in cls._tree_types:
            cls._tree_types[key] = TreeType(name, color, texture)
            print(f"Создан новый тип дерева: {key}")
        
        return cls._tree_types[key]
    
    @classmethod
    def get_total_types(cls) -> int:
        return len(cls._tree_types)


class Tree:
    """Контекст - индивидуальное состояние дерева"""
    
    def __init__(self, x: int, y: int, tree_type: TreeType):
        self.x = x
        self.y = y
        self.tree_type = tree_type
    
    def draw(self):
        self.tree_type.draw(self.x, self.y)


class Forest:
    """Коллекция деревьев"""
    
    def __init__(self):
        self.trees = []
    
    def plant_tree(self, x: int, y: int, name: str, color: str, texture: str):
        """Посадить дерево"""
        tree_type = TreeFactory.get_tree_type(name, color, texture)
        tree = Tree(x, y, tree_type)
        self.trees.append(tree)
    
    def draw(self):
        """Нарисовать лес"""
        for tree in self.trees:
            tree.draw()


def demo():
    """Демонстрация работы паттерна Flyweight"""
    print("=== Паттерн Flyweight (Приспособленец) ===\n")
    
    forest = Forest()
    
    # Сажаем много деревьев (используются только 3 типа)
    print("Посадка деревьев:")
    forest.plant_tree(1, 1, "Дуб", "зеленый", "грубая")
    forest.plant_tree(2, 3, "Дуб", "зеленый", "грубая")
    forest.plant_tree(5, 2, "Сосна", "темно-зеленый", "игольчатая")
    forest.plant_tree(3, 7, "Дуб", "зеленый", "грубая")
    forest.plant_tree(8, 4, "Береза", "белый", "гладкая")
    forest.plant_tree(6, 9, "Сосна", "темно-зеленый", "игольчатая")
    
    print(f"\nВсего уникальных типов деревьев: {TreeFactory.get_total_types()}")
    print(f"Всего деревьев в лесу: {len(forest.trees)}")
    
    print("\nРисуем лес:")
    forest.draw()


if __name__ == "__main__":
    demo()
