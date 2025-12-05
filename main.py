#!/usr/bin/env python3
"""
Демонстрация всех структурных паттернов проектирования
"""

import adapter
import bridge
import composite
import decorator
import facade
import flyweight
import proxy


def main():
    """Запуск демонстрации всех паттернов"""
    
    print("\n" + "="*60)
    print(" СТРУКТУРНЫЕ ПАТТЕРНЫ ПРОЕКТИРОВАНИЯ ")
    print("="*60 + "\n")
    
    patterns = [
        adapter,
        bridge,
        composite,
        decorator,
        facade,
        flyweight,
        proxy
    ]
    
    for i, pattern_module in enumerate(patterns, 1):
        pattern_module.demo()
        if i < len(patterns):
            print("\n" + "="*60 + "\n")


if __name__ == "__main__":
    main()
