"""
Adapter Pattern (Адаптер)

Позволяет объектам с несовместимыми интерфейсами работать вместе.
Преобразует интерфейс одного класса в интерфейс другого, ожидаемый клиентами.
"""


class EuropeanPlug:
    """Европейская розетка (220V)"""
    
    def connect_european(self):
        return "Подключено к европейской розетке (220V)"


class USAPlug:
    """Американская розетка (110V)"""
    
    def connect_usa(self):
        return "Подключено к американской розетке (110V)"


class PlugAdapter:
    """Адаптер для подключения европейских устройств к американским розеткам"""
    
    def __init__(self, usa_plug: USAPlug):
        self.usa_plug = usa_plug
    
    def connect_european(self):
        # Адаптируем американский интерфейс к европейскому
        result = self.usa_plug.connect_usa()
        return f"{result} -> Преобразовано адаптером для европейского устройства"


def demo():
    """Демонстрация работы паттерна Adapter"""
    print("=== Паттерн Adapter (Адаптер) ===\n")
    
    # Прямое подключение к европейской розетке
    european_plug = EuropeanPlug()
    print(f"Европейское устройство: {european_plug.connect_european()}")
    
    # Подключение через адаптер к американской розетке
    usa_plug = USAPlug()
    adapter = PlugAdapter(usa_plug)
    print(f"Европейское устройство через адаптер: {adapter.connect_european()}")


if __name__ == "__main__":
    demo()
