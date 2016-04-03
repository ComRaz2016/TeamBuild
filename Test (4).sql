-- phpMyAdmin SQL Dump
-- version 4.0.10deb1
-- http://www.phpmyadmin.net
--
-- Хост: localhost
-- Время создания: Апр 03 2016 г., 20:14
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
-- Структура таблицы `Instuction_zone`
--

CREATE TABLE IF NOT EXISTS `Instuction_zone` (
  `pk_instruction` decimal(38,0) NOT NULL,
  `pk_order` decimal(38,0) NOT NULL,
  PRIMARY KEY (`pk_instruction`,`pk_order`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `Instuction_zone`
--

INSERT INTO `Instuction_zone` (`pk_instruction`, `pk_order`) VALUES
(1, 22),
(1, 37),
(2, 22),
(2, 34),
(4, 33);

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
  `volume` decimal(10,0) DEFAULT NULL,
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
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=46 ;

--
-- Дамп данных таблицы `Order`
--

INSERT INTO `Order` (`pk_order`, `nomer`, `volume`, `date_time`, `adress`, `contact`, `number_contact`, `comment`, `Numberzone`, `Exstendway`, `worker`, `cost_order`, `pk_status`, `pk_material`, `pk_measure`) VALUES
(30, 100015, 1, '26.03.2016 15:22', 'вапавпвап', 'апавпавп', '353455', '', 1, 0, 0, 1288, 2, 1, 1),
(31, 100016, 1, '25.03.2016 20:24', 'ывавыаыва', 'ыпвававыа', '324324324', '', 1, 0, 0, 2070, 4, 1, 2),
(32, 100017, 1, '26.03.2016 15:25', 'ваыывавыа', 'цкцукуцк', '324234324', '', 1, 0, 0, 1288, 2, 1, 1),
(33, 100018, 1, '29.03.2016 12:20', 'Барни', 'Каака', '+453435435', '', 3, 0, 0, 8280, 2, 1, 1),
(34, 100019, 23, '30.03.2016 01:31', 'Барнаул', 'Воробей', '+7678687', '', 2, 0, 0, 31446, 3, 2, 1),
(35, 100020, 1, '29.03.2016 03:34', 'Рубцовск', 'Соловей', '+45234324', '', 2, 0, 1, 3812, 1, 2, 1),
(36, 100021, 1, '03.04.2016 01:40', 'Бийск', 'Сорока', '+234324324', '', 4, 7, 1, 5042, 2, 2, 1),
(37, 100022, 23, '29.03.2016 15:29', 'Барнаул', 'Шахов', '+435324234', '', 4, 1, 1, 41504, 1, 2, 1),
(38, 100023, 440, '30.03.2016 12:32', 'Бийск', 'Шахов', '+243432324', '', 3, 0, 4, 303255, 3, 10, 2),
(39, 100024, 29, '30.03.2016 16:05', 'Барнаул', 'Шахов', '+435435345', '', 1, 0, 0, 36685, 2, 1, 1),
(40, 100025, 11, '01.04.2016 00:00', 'Барнаул', 'Шахов', '+324324234', '', 2, 0, 5, 22655, 2, 8, 1),
(41, 100026, 1245, '29.03.2016 20:15', 'Барнаул', 'Шахов', '+324324234', '', 1, 0, 1, 813625, 1, 10, 2),
(42, 100027, 21, '29.03.2016 21:17', 'Барнаул', 'Шахов', '+3245235324', '', 1, 0, 1, 27506, 1, 2, 1),
(43, 100028, 12, '29.03.2016 22:19', 'Барни', 'Шахов+342353477', '+324324324', '', 3, 0, 5, 19780, 1, 1, 1),
(44, 100029, 1, '29.03.2016 23:23', 'Барнаул', 'Шахов', '+5423132342', '', 1, 0, 5, 5750, 1, 1, 1),
(45, 100030, 12, '03.04.2016 16:24', 'Бийск', 'Шахов', '+342384723', '', 3, 0, 1, 24483, 2, 2, 1);

-- --------------------------------------------------------

--
-- Структура таблицы `order_car`
--

CREATE TABLE IF NOT EXISTS `order_car` (
  `pk_car` int(11) NOT NULL,
  `pk_order` int(11) NOT NULL,
  `count_trip` int(11) DEFAULT NULL,
  PRIMARY KEY (`pk_car`,`pk_order`),
  KEY `Relationship6` (`pk_order`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `order_car`
--

INSERT INTO `order_car` (`pk_car`, `pk_order`, `count_trip`) VALUES
(1, 30, 1),
(1, 32, 1),
(1, 34, 2),
(1, 35, 1),
(1, 36, 1),
(1, 39, 2),
(1, 42, 2),
(1, 43, 1),
(2, 31, 1),
(2, 38, 4),
(4, 37, 2),
(4, 44, 1),
(5, 38, 1),
(5, 40, 1),
(5, 41, 2),
(5, 45, 1);

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
(1, 100031);

-- --------------------------------------------------------

--
-- Структура таблицы `order_status`
--

CREATE TABLE IF NOT EXISTS `order_status` (
  `pk_status` int(11) NOT NULL AUTO_INCREMENT,
  `name_status` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`pk_status`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=5 ;

--
-- Дамп данных таблицы `order_status`
--

INSERT INTO `order_status` (`pk_status`, `name_status`) VALUES
(1, 'Active'),
(2, 'Inactive'),
(3, 'Complete'),
(4, 'Cancel');

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

-- --------------------------------------------------------

--
-- Структура таблицы `Status`
--

CREATE TABLE IF NOT EXISTS `Status` (
  `pk_status` int(11) NOT NULL AUTO_INCREMENT,
  `status_name` varchar(300) NOT NULL,
  PRIMARY KEY (`pk_status`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=4 ;

--
-- Дамп данных таблицы `Status`
--

INSERT INTO `Status` (`pk_status`, `status_name`) VALUES
(1, 'Active'),
(2, 'Inactive'),
(3, 'Complete');

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
