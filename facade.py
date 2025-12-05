"""
Facade Pattern (Фасад)

Предоставляет унифицированный интерфейс к набору интерфейсов в подсистеме.
Определяет высокоуровневый интерфейс, упрощающий использование подсистемы.
"""


class CPU:
    """Подсистема - Процессор"""
    
    def freeze(self):
        print("CPU: Заморозка процессора")
    
    def jump(self, position: str):
        print(f"CPU: Переход к позиции {position}")
    
    def execute(self):
        print("CPU: Выполнение команд")


class Memory:
    """Подсистема - Память"""
    
    def load(self, position: str, data: str):
        print(f"Memory: Загрузка данных '{data}' в позицию {position}")


class HardDrive:
    """Подсистема - Жесткий диск"""
    
    def read(self, sector: str, size: int) -> str:
        print(f"HardDrive: Чтение {size} байт из сектора {sector}")
        return f"данные из сектора {sector}"


class ComputerFacade:
    """Фасад - упрощенный интерфейс для запуска компьютера"""
    
    def __init__(self):
        self.cpu = CPU()
        self.memory = Memory()
        self.hard_drive = HardDrive()
    
    def start(self):
        """Запуск компьютера - один простой метод вместо множества сложных вызовов"""
        print("=== Запуск компьютера ===")
        self.cpu.freeze()
        data = self.hard_drive.read("0x00", 1024)
        self.memory.load("0x00", data)
        self.cpu.jump("0x00")
        self.cpu.execute()
        print("=== Компьютер запущен ===\n")


def demo():
    """Демонстрация работы паттерна Facade"""
    print("=== Паттерн Facade (Фасад) ===\n")
    
    # Без фасада пришлось бы вызывать множество методов
    print("Без фасада:")
    cpu = CPU()
    memory = Memory()
    hard_drive = HardDrive()
    
    cpu.freeze()
    data = hard_drive.read("0x00", 1024)
    memory.load("0x00", data)
    cpu.jump("0x00")
    cpu.execute()
    print()
    
    # С фасадом все просто
    print("С фасадом:")
    computer = ComputerFacade()
    computer.start()


if __name__ == "__main__":
    demo()
