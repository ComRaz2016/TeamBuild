-- phpMyAdmin SQL Dump
-- version 4.0.10deb1
-- http://www.phpmyadmin.net
--
-- Хост: localhost
-- Время создания: Мар 24 2016 г., 18:06
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
(1, 'Камаз', 'е8934а', 0, 1, 7, 1000, 2000, 3000, 10, 1),
(2, 'Газель', 'т3434тт', 1, 0, 3, 800, 1400, 2000, 10, 2),
(3, 'Hino', 'x989xx', 1, 1, 10, 1200, 2000, 3100, 11, 2),
(4, 'Man', 'y675yy', 0, 1, 20, 2500, 4000, 6000, 20, 2),
(5, 'DAF FX', 't743tt', 1, 1, 30, 5000, 6600, 8100, 25, 3);

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
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=4 ;

--
-- Дамп данных таблицы `Driver`
--

INSERT INTO `Driver` (`pk_driver`, `fio_driver`, `nomber_driver`, `tel_number_driver`) VALUES
(1, 'Васильев', 'о3847оо', '+98435324234'),
(2, 'Петров', 'аа67еаа', '+8473823409'),
(3, 'Воробьев', 'a585rrw', '+832479832');

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
(2, 1),
(3, 1),
(5, 1),
(1, 2),
(3, 2),
(4, 2),
(5, 2);

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
(1, 19),
(1, 22),
(2, 19),
(2, 22),
(3, 20),
(4, 20);

-- --------------------------------------------------------

--
-- Структура таблицы `Material`
--

CREATE TABLE IF NOT EXISTS `Material` (
  `pk_material` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`pk_material`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=3 ;

--
-- Дамп данных таблицы `Material`
--

INSERT INTO `Material` (`pk_material`, `name`) VALUES
(1, 'Речной песок'),
(2, 'Песок');

-- --------------------------------------------------------

--
-- Структура таблицы `Measure`
--

CREATE TABLE IF NOT EXISTS `Measure` (
  `pk_measure` int(11) NOT NULL AUTO_INCREMENT,
  `Nazv` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`pk_measure`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=4 ;

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
  `volume` int(11) DEFAULT NULL,
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
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=23 ;

--
-- Дамп данных таблицы `Order`
--

INSERT INTO `Order` (`pk_order`, `nomer`, `volume`, `date_time`, `adress`, `contact`, `number_contact`, `comment`, `Numberzone`, `Exstendway`, `worker`, `cost_order`, `pk_status`, `pk_material`, `pk_measure`) VALUES
(3, 100002, 1, '24.03.2016 18:34', 'Барнаул', 'Шахов', '+34723324', '', 1, 0, 1, 1633, 1, 1, 1),
(4, 100002, 1, '24.03.2016 20:15', 'Барни', 'Шахов', '+3248324', 'Всё ок', 2, 0, 0, 2587, 1, 2, 1),
(5, 100002, 67, '24.03.2016 20:15', 'Барни', 'Шахов', '+3248324', 'Всё ок', 2, 0, 5, 81075, 1, 1, 2),
(6, 100002, 1, '24.03.2016 18:25', 'Бийск', 'Шахов', '+84758324', '', 4, 67, 1, 4703, 1, 1, 1),
(7, 100002, 1, '25.03.2016 18:25', 'Бийск', 'Шахов', '+84758324', '', 2, 0, 1, 2783, 2, 1, 1),
(8, 100002, 1, '25.04.2016 18:25', 'Бийск', 'Шахов', '+84758324', '', 2, 0, 1, 2783, 2, 1, 1),
(9, 100002, 42, '25.04.2016 18:25', 'Бийск', 'Шахов', '+84758324', '', 3, 0, 1, 50945, 2, 1, 2),
(10, 100002, 5, '24.03.2016 19:54', 'Рубцовск', 'Петров', '+9384204', '', 4, 67, 1, 5255, 1, 1, 1),
(11, 100002, 56, '24.03.2016 20:56', 'Рубцовск', 'Петров', '+345234324', '', 3, 0, 1, 35673, 1, 1, 1),
(12, 100003, 7, '24.03.2016 20:00', 'Барнаул', 'Шахов', '+3284732', '', 2, 0, 1, 3611, 1, 1, 1),
(13, 100003, 1, '24.03.2016 21:02', 'ывавыа', 'выаыва', 'ываыва', '', 1, 0, 1, 1782, 1, 2, 1),
(14, 100004, 1, '24.03.2016 22:06', 'Барни', 'ПЕтров', '+234324', 'ывавыа', 3, 0, 0, 3588, 1, 1, 1),
(15, 100005, 1, '25.03.2016 22:07', 'выаываыва', 'куцкуц', '456456', 'аывавыаыва', 4, 56, 1, 8850, 2, 1, 1),
(16, 100006, 54, '26.03.2016 17:10', 'ававыавы', 'рапрпа', '54654654', '', 1, 0, 1, 26335, 2, 2, 1),
(17, 100007, 1, '24.03.2016 20:26', 'ываываыва', 'йцуйцуйцу', '3243243242', 'аывавыавыаыва', 2, 0, 1, 3795, 1, 1, 2),
(18, 100007, 45, '24.03.2016 20:27', 'вапавпвап', 'ауыаывавы', '234324234', 'фывфыв', 3, 0, 1, 14260, 1, 2, 2),
(19, 100007, 1, '24.03.2016 21:29', 'ывавыаыв', 'выавыаыв', '45345435', 'аываываыв', 3, 0, 1, 4140, 1, 2, 2),
(20, 100008, 1, '24.03.2016 19:30', 'ываываа', 'выаываыв', '45345435', '', 3, 0, 0, 138, 1, 1, 1),
(21, 100009, 1, '24.03.2016 20:53', 'Барнаул', 'Шахов', '+324324234', '', 3, 0, 0, 7153, 1, 1, 1),
(22, 100010, 1, '25.03.2016 18:03', 'Новоалтайск', 'Воробьев', '+83247328', '', 1, 0, 5, 3243, 2, 1, 1);

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
(1, 13, 1),
(1, 14, 1),
(1, 15, 1),
(1, 16, 4),
(1, 21, 1),
(3, 15, 1),
(3, 17, 1),
(3, 18, 1),
(3, 19, 1),
(3, 21, 1),
(3, 22, 1),
(5, 16, 1);

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
(1, 100011);

-- --------------------------------------------------------

--
-- Структура таблицы `order_status`
--

CREATE TABLE IF NOT EXISTS `order_status` (
  `pk_status` int(11) NOT NULL AUTO_INCREMENT,
  `name_status` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`pk_status`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=4 ;

--
-- Дамп данных таблицы `order_status`
--

INSERT INTO `order_status` (`pk_status`, `name_status`) VALUES
(1, 'Active'),
(2, 'Inactive'),
(3, 'Complete');

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
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=2 ;

--
-- Дамп данных таблицы `Provider`
--

INSERT INTO `Provider` (`pk_provider`, `name_firm`, `adress_firm`, `tel_number_firm`) VALUES
(1, 'ОАО Песок', 'ул. Ленина, 123', '+7812743981');

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
(1, 1, 1000, 120),
(1, 2, 200, 250);

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
