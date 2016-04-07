-- phpMyAdmin SQL Dump
-- version 4.0.10deb1
-- http://www.phpmyadmin.net
--
-- Хост: localhost
-- Время создания: Апр 07 2016 г., 15:16
-- Версия сервера: 5.6.29
-- Версия PHP: 5.5.9-1ubuntu4.14

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- База данных: `Test`
--

-- --------------------------------------------------------

--
-- Структура таблицы `Car`
--

CREATE TABLE IF NOT EXISTS `Car` (
  `pk_car` int(11) NOT NULL AUTO_INCREMENT,
  `mark_car` varchar(300) DEFAULT NULL,
  `regist_number` varchar(300) DEFAULT NULL,
  `delivery_bag` int(11) DEFAULT NULL,
  `delivery_bulk` int(11) DEFAULT NULL,
  `tonnage` decimal(38,0) DEFAULT NULL,
  `Costfistzone` decimal(38,0) DEFAULT NULL,
  `Costsecondzone` decimal(38,0) DEFAULT NULL,
  `Costthirdzone` decimal(38,0) DEFAULT NULL,
  `Costdopkm` decimal(38,0) DEFAULT NULL,
  `pk_driver` int(11) DEFAULT NULL,
  PRIMARY KEY (`pk_car`),
  KEY `IX_Relationship2` (`pk_driver`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=6 ;

--
-- Дамп данных таблицы `Car`
--

INSERT INTO `Car` (`pk_car`, `mark_car`, `regist_number`, `delivery_bag`, `delivery_bulk`, `tonnage`, `Costfistzone`, `Costsecondzone`, `Costthirdzone`, `Costdopkm`, `pk_driver`) VALUES
(1, 'Камаз 5410', 'к993ам97', 0, 1, 15, 1000, 2000, 3000, 10, 2),
(2, 'Газель', 'т3434тт', 1, 0, 3, 800, 1400, 2000, 10, 2),
(4, 'MAN', 'у675уу', 1, 0, 25, 2500, 4000, 6000, 20, 2),
(5, 'Mersedes', 'м342ер33', 0, 1, 41, 502, 661, 810, 25, 5);

-- --------------------------------------------------------

--
-- Структура таблицы `Driver`
--

CREATE TABLE IF NOT EXISTS `Driver` (
  `pk_driver` int(11) NOT NULL AUTO_INCREMENT,
  `fio_driver` varchar(300) DEFAULT NULL,
  `nomber_driver` varchar(300) DEFAULT NULL,
  `tel_number_driver` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`pk_driver`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=6 ;

--
-- Дамп данных таблицы `Driver`
--

INSERT INTO `Driver` (`pk_driver`, `fio_driver`, `nomber_driver`, `tel_number_driver`) VALUES
(1, 'Васильева', 'а88а8аб', '+98435324235'),
(2, 'Петров', 'вб88б8б', '+090837876'),
(3, 'Воробьев', 'в88в8в', '+832479832'),
(4, 'Сидоров', 'с548ид', '+8745845435'),
(5, 'Макарова', 'м876м', '+435345345');

-- --------------------------------------------------------

--
-- Структура таблицы `instruction`
--

CREATE TABLE IF NOT EXISTS `instruction` (
  `pk_instruction` int(11) NOT NULL AUTO_INCREMENT,
  `desc_instruction` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`pk_instruction`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=5 ;

--
-- Дамп данных таблицы `instruction`
--

INSERT INTO `instruction` (`pk_instruction`, `desc_instruction`) VALUES
(1, 'Compact'),
(2, 'Tipper'),
(3, 'Onboard'),
(4, 'Selfloader');

-- --------------------------------------------------------

--
-- Структура таблицы `instruction_car`
--

CREATE TABLE IF NOT EXISTS `instruction_car` (
  `pk_car` int(11) NOT NULL,
  `pk_instruction` int(11) NOT NULL,
  PRIMARY KEY (`pk_car`,`pk_instruction`),
  KEY `Relationship9` (`pk_instruction`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `instruction_car`
--

INSERT INTO `instruction_car` (`pk_car`, `pk_instruction`) VALUES
(1, 1),
(2, 1),
(4, 2),
(5, 2),
(4, 3),
(5, 3),
(1, 4),
(2, 4);

-- --------------------------------------------------------

--
-- Структура таблицы `Material`
--

CREATE TABLE IF NOT EXISTS `Material` (
  `pk_material` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`pk_material`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=11 ;

--
-- Дамп данных таблицы `Material`
--

INSERT INTO `Material` (`pk_material`, `name`) VALUES
(1, 'Речная галька'),
(2, 'Щебень'),
(7, 'Песок'),
(8, 'Речной песок'),
(9, 'Морской песок'),
(10, 'Клей  плиточный');

-- --------------------------------------------------------

--
-- Структура таблицы `Measure`
--

CREATE TABLE IF NOT EXISTS `Measure` (
  `pk_measure` int(11) NOT NULL AUTO_INCREMENT,
  `Nazv` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`pk_measure`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=3 ;

--
-- Дамп данных таблицы `Measure`
--

INSERT INTO `Measure` (`pk_measure`, `Nazv`) VALUES
(1, 'Bulk'),
(2, 'Bag');

-- --------------------------------------------------------

--
-- Структура таблицы `Order`
--

CREATE TABLE IF NOT EXISTS `Order` (
  `pk_order` int(11) NOT NULL AUTO_INCREMENT,
  `nomer` int(11) DEFAULT NULL,
  `volume` decimal(10,3) DEFAULT NULL,
  `date_time` varchar(300) DEFAULT NULL,
  `adress` varchar(300) DEFAULT NULL,
  `contact` varchar(300) DEFAULT NULL,
  `number_contact` varchar(300) DEFAULT NULL,
  `comment` varchar(500) DEFAULT NULL,
  `Numberzone` int(11) DEFAULT '1',
  `Exstendway` decimal(38,0) DEFAULT NULL,
  `worker` int(11) DEFAULT NULL,
  `cost_order` int(11) DEFAULT NULL,
  `pk_status` int(11) DEFAULT NULL,
  `pk_material` int(11) DEFAULT NULL,
  `pk_measure` int(11) DEFAULT NULL,
  PRIMARY KEY (`pk_order`),
  KEY `IX_Relationship11` (`pk_status`),
  KEY `IX_Relationship13` (`pk_material`),
  KEY `IX_Relationship17` (`pk_measure`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=60 ;

--
-- Дамп данных таблицы `Order`
--

INSERT INTO `Order` (`pk_order`, `nomer`, `volume`, `date_time`, `adress`, `contact`, `number_contact`, `comment`, `Numberzone`, `Exstendway`, `worker`, `cost_order`, `pk_status`, `pk_material`, `pk_measure`) VALUES
(59, 100040, 30.000, '07.04.2016 23:14', 'сюда', 'Кристалев', '333000', 'все ок', 1, 0, 4, 8799, 3, 2, 1);

-- --------------------------------------------------------

--
-- Структура таблицы `order_car`
--

CREATE TABLE IF NOT EXISTS `order_car` (
  `pk_car` int(11) NOT NULL,
  `pk_order` int(11) NOT NULL,
  `count_trip` int(11) DEFAULT NULL,
  `volume_car` decimal(10,2) NOT NULL,
  PRIMARY KEY (`pk_car`,`pk_order`),
  KEY `Relationship6` (`pk_order`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `order_car`
--

INSERT INTO `order_car` (`pk_car`, `pk_order`, `count_trip`, `volume_car`) VALUES
(1, 59, 1, 15.00),
(5, 59, 1, 15.00);

-- --------------------------------------------------------

--
-- Структура таблицы `order_instruction`
--

CREATE TABLE IF NOT EXISTS `order_instruction` (
  `pk_instruction` int(11) NOT NULL,
  `pk_order` int(11) NOT NULL,
  PRIMARY KEY (`pk_instruction`,`pk_order`),
  KEY `Relationship12` (`pk_order`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Структура таблицы `order_number`
--

CREATE TABLE IF NOT EXISTS `order_number` (
  `pk_order_number` int(11) NOT NULL AUTO_INCREMENT,
  `number_order` bigint(20) NOT NULL,
  PRIMARY KEY (`pk_order_number`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=2 ;

--
-- Дамп данных таблицы `order_number`
--

INSERT INTO `order_number` (`pk_order_number`, `number_order`) VALUES
(1, 100041);

-- --------------------------------------------------------

--
-- Структура таблицы `order_status`
--

CREATE TABLE IF NOT EXISTS `order_status` (
  `pk_status` int(11) NOT NULL AUTO_INCREMENT,
  `name_status` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`pk_status`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=8 ;

--
-- Дамп данных таблицы `order_status`
--

INSERT INTO `order_status` (`pk_status`, `name_status`) VALUES
(1, 'Wait'),
(2, 'RidesOnPickup'),
(3, 'Loaded'),
(4, 'EffectedDelivery'),
(5, 'DeliveryIsMade'),
(6, 'Complete'),
(7, 'Raw');

-- --------------------------------------------------------

--
-- Структура таблицы `Provider`
--

CREATE TABLE IF NOT EXISTS `Provider` (
  `pk_provider` int(11) NOT NULL AUTO_INCREMENT,
  `name_firm` varchar(300) DEFAULT NULL,
  `adress_firm` varchar(300) DEFAULT NULL,
  `tel_number_firm` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`pk_provider`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=13 ;

--
-- Дамп данных таблицы `Provider`
--

INSERT INTO `Provider` (`pk_provider`, `name_firm`, `adress_firm`, `tel_number_firm`) VALUES
(1, 'ЗАО Песочек', 'ул. Ленина, 126', '+7812743999'),
(12, 'ОАО Щебень', 'Барнаул', '+3453485');

-- --------------------------------------------------------

--
-- Структура таблицы `provider_material`
--

CREATE TABLE IF NOT EXISTS `provider_material` (
  `pk_provider` int(11) NOT NULL,
  `pk_material` int(11) NOT NULL,
  `cost_bag` int(11) DEFAULT NULL,
  `cost_tonna` int(11) DEFAULT NULL,
  PRIMARY KEY (`pk_provider`,`pk_material`),
  KEY `Relationship4` (`pk_material`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `provider_material`
--

INSERT INTO `provider_material` (`pk_provider`, `pk_material`, `cost_bag`, `cost_tonna`) VALUES
(1, 1, 30, 1000),
(1, 2, 180, 250),
(12, 2, 10, 80),
(12, 8, 60, 1000),
(12, 9, 324, 234),
(12, 10, 560, 34);

--
-- Ограничения внешнего ключа сохраненных таблиц
--

--
-- Ограничения внешнего ключа таблицы `Car`
--
ALTER TABLE `Car`
  ADD CONSTRAINT `Relationship1` FOREIGN KEY (`pk_driver`) REFERENCES `Driver` (`pk_driver`);

--
-- Ограничения внешнего ключа таблицы `instruction_car`
--
ALTER TABLE `instruction_car`
  ADD CONSTRAINT `Relationship8` FOREIGN KEY (`pk_car`) REFERENCES `Car` (`pk_car`),
  ADD CONSTRAINT `Relationship9` FOREIGN KEY (`pk_instruction`) REFERENCES `instruction` (`pk_instruction`);

--
-- Ограничения внешнего ключа таблицы `Order`
--
ALTER TABLE `Order`
  ADD CONSTRAINT `Relationship10` FOREIGN KEY (`pk_status`) REFERENCES `order_status` (`pk_status`),
  ADD CONSTRAINT `Relationship2` FOREIGN KEY (`pk_measure`) REFERENCES `Measure` (`pk_measure`),
  ADD CONSTRAINT `Relationship7` FOREIGN KEY (`pk_material`) REFERENCES `Material` (`pk_material`);

--
-- Ограничения внешнего ключа таблицы `order_car`
--
ALTER TABLE `order_car`
  ADD CONSTRAINT `Relationship5` FOREIGN KEY (`pk_car`) REFERENCES `Car` (`pk_car`),
  ADD CONSTRAINT `Relationship6` FOREIGN KEY (`pk_order`) REFERENCES `Order` (`pk_order`);

--
-- Ограничения внешнего ключа таблицы `order_instruction`
--
ALTER TABLE `order_instruction`
  ADD CONSTRAINT `Relationship11` FOREIGN KEY (`pk_instruction`) REFERENCES `instruction` (`pk_instruction`),
  ADD CONSTRAINT `Relationship12` FOREIGN KEY (`pk_order`) REFERENCES `Order` (`pk_order`);

--
-- Ограничения внешнего ключа таблицы `provider_material`
--
ALTER TABLE `provider_material`
  ADD CONSTRAINT `Relationship3` FOREIGN KEY (`pk_provider`) REFERENCES `Provider` (`pk_provider`),
  ADD CONSTRAINT `Relationship4` FOREIGN KEY (`pk_material`) REFERENCES `Material` (`pk_material`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
