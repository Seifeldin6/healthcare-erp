// GENERATED CODE - DO NOT MODIFY BY HAND
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

import 'intl/messages_all.dart';

// **************************************************************************
// Generator: Flutter Intl IDE plugin
// Made by Localizely
// **************************************************************************

// ignore_for_file: non_constant_identifier_names, lines_longer_than_80_chars
// ignore_for_file: join_return_with_assignment, prefer_final_in_for_each
// ignore_for_file: avoid_redundant_argument_values, avoid_escaping_inner_quotes

class S {
  S();

  static S? _current;

  static S get current {
    assert(
      _current != null,
      'No instance of S was loaded. Try to initialize the S delegate before accessing S.current.',
    );
    return _current!;
  }

  static const AppLocalizationDelegate delegate = AppLocalizationDelegate();

  static Future<S> load(Locale locale) {
    final name = (locale.countryCode?.isEmpty ?? false)
        ? locale.languageCode
        : locale.toString();
    final localeName = Intl.canonicalizedLocale(name);
    return initializeMessages(localeName).then((_) {
      Intl.defaultLocale = localeName;
      final instance = S();
      S._current = instance;

      return instance;
    });
  }

  static S of(BuildContext context) {
    final instance = S.maybeOf(context);
    assert(
      instance != null,
      'No instance of S present in the widget tree. Did you add S.delegate in localizationsDelegates?',
    );
    return instance!;
  }

  static S? maybeOf(BuildContext context) {
    return Localizations.of<S>(context, S);
  }

  /// `Shifa`
  String get appName {
    return Intl.message('Shifa', name: 'appName', desc: '', args: []);
  }

  /// `شفاء`
  String get appNameSecondary {
    return Intl.message('شفاء', name: 'appNameSecondary', desc: '', args: []);
  }

  /// `Trusted pharmacy care, wherever you are.`
  String get splashTagline {
    return Intl.message(
      'Trusted pharmacy care, wherever you are.',
      name: 'splashTagline',
      desc: '',
      args: [],
    );
  }

  /// `TELE-PHARMACY`
  String get splashFooterEn {
    return Intl.message(
      'TELE-PHARMACY',
      name: 'splashFooterEn',
      desc: '',
      args: [],
    );
  }

  /// `صيدلية عن بُعد`
  String get splashFooterAr {
    return Intl.message(
      'صيدلية عن بُعد',
      name: 'splashFooterAr',
      desc: '',
      args: [],
    );
  }

  /// `Skip`
  String get onboardingSkip {
    return Intl.message('Skip', name: 'onboardingSkip', desc: '', args: []);
  }

  /// `Next`
  String get onboardingNext {
    return Intl.message('Next', name: 'onboardingNext', desc: '', args: []);
  }

  /// `Create account`
  String get onboardingCreateAccount {
    return Intl.message(
      'Create account',
      name: 'onboardingCreateAccount',
      desc: '',
      args: [],
    );
  }

  /// `Already have an account?`
  String get onboardingAlreadyHaveAccount {
    return Intl.message(
      'Already have an account?',
      name: 'onboardingAlreadyHaveAccount',
      desc: '',
      args: [],
    );
  }

  /// `Sign in`
  String get onboardingSignIn {
    return Intl.message(
      'Sign in',
      name: 'onboardingSignIn',
      desc: '',
      args: [],
    );
  }

  /// `Find a verified\npharmacist`
  String get onboardingStep1Title {
    return Intl.message(
      'Find a verified\npharmacist',
      name: 'onboardingStep1Title',
      desc: '',
      args: [],
    );
  }

  /// `Browse clinical and herbal pharmacists, see who is available now and what a consultation costs.`
  String get onboardingStep1Description {
    return Intl.message(
      'Browse clinical and herbal pharmacists, see who is available now and what a consultation costs.',
      name: 'onboardingStep1Description',
      desc: '',
      args: [],
    );
  }

  /// `Consult by chat or video`
  String get onboardingStep2Title {
    return Intl.message(
      'Consult by chat or video',
      name: 'onboardingStep2Title',
      desc: '',
      args: [],
    );
  }

  /// `Describe your symptoms and pay only after the pharmacist accepts. Then chat freely or join a video call when they start one.`
  String get onboardingStep2Description {
    return Intl.message(
      'Describe your symptoms and pay only after the pharmacist accepts. Then chat freely or join a video call when they start one.',
      name: 'onboardingStep2Description',
      desc: '',
      args: [],
    );
  }

  /// `Your prescription and\nhealth, in one place`
  String get onboardingStep3Title {
    return Intl.message(
      'Your prescription and\nhealth, in one place',
      name: 'onboardingStep3Title',
      desc: '',
      args: [],
    );
  }

  /// `Keep allergies, conditions and medications up to date. You decide when a pharmacist can see them.`
  String get onboardingStep3Description {
    return Intl.message(
      'Keep allergies, conditions and medications up to date. You decide when a pharmacist can see them.',
      name: 'onboardingStep3Description',
      desc: '',
      args: [],
    );
  }

  /// `Search pharmacist`
  String get pharmacistMockupSearchHint {
    return Intl.message(
      'Search pharmacist',
      name: 'pharmacistMockupSearchHint',
      desc: '',
      args: [],
    );
  }

  /// `All`
  String get pharmacistMockupFilterAll {
    return Intl.message(
      'All',
      name: 'pharmacistMockupFilterAll',
      desc: '',
      args: [],
    );
  }

  /// `Clinical`
  String get pharmacistMockupFilterClinical {
    return Intl.message(
      'Clinical',
      name: 'pharmacistMockupFilterClinical',
      desc: '',
      args: [],
    );
  }

  /// `Herbal`
  String get pharmacistMockupFilterHerbal {
    return Intl.message(
      'Herbal',
      name: 'pharmacistMockupFilterHerbal',
      desc: '',
      args: [],
    );
  }

  /// `Dr. Mariam Adel`
  String get pharmacistMockupName1 {
    return Intl.message(
      'Dr. Mariam Adel',
      name: 'pharmacistMockupName1',
      desc: '',
      args: [],
    );
  }

  /// `MA`
  String get pharmacistMockupInitials1 {
    return Intl.message(
      'MA',
      name: 'pharmacistMockupInitials1',
      desc: '',
      args: [],
    );
  }

  /// `Clinical Pharmacy`
  String get pharmacistMockupSpecialty1 {
    return Intl.message(
      'Clinical Pharmacy',
      name: 'pharmacistMockupSpecialty1',
      desc: '',
      args: [],
    );
  }

  /// `150`
  String get pharmacistMockupPrice1 {
    return Intl.message(
      '150',
      name: 'pharmacistMockupPrice1',
      desc: '',
      args: [],
    );
  }

  /// `EGP`
  String get pharmacistMockupCurrency {
    return Intl.message(
      'EGP',
      name: 'pharmacistMockupCurrency',
      desc: '',
      args: [],
    );
  }

  /// `Dr. Youssef Ali`
  String get pharmacistMockupName2 {
    return Intl.message(
      'Dr. Youssef Ali',
      name: 'pharmacistMockupName2',
      desc: '',
      args: [],
    );
  }

  /// `YA`
  String get pharmacistMockupInitials2 {
    return Intl.message(
      'YA',
      name: 'pharmacistMockupInitials2',
      desc: '',
      args: [],
    );
  }

  /// `Herbal & Complementary`
  String get pharmacistMockupSpecialty2 {
    return Intl.message(
      'Herbal & Complementary',
      name: 'pharmacistMockupSpecialty2',
      desc: '',
      args: [],
    );
  }

  /// `120`
  String get pharmacistMockupPrice2 {
    return Intl.message(
      '120',
      name: 'pharmacistMockupPrice2',
      desc: '',
      args: [],
    );
  }

  /// `NS`
  String get pharmacistMockupSkeletonInitials {
    return Intl.message(
      'NS',
      name: 'pharmacistMockupSkeletonInitials',
      desc: '',
      args: [],
    );
  }

  /// `Available`
  String get pharmacistMockupAvailable {
    return Intl.message(
      'Available',
      name: 'pharmacistMockupAvailable',
      desc: '',
      args: [],
    );
  }

  /// `MA`
  String get chatMockupPharmacistInitials {
    return Intl.message(
      'MA',
      name: 'chatMockupPharmacistInitials',
      desc: '',
      args: [],
    );
  }

  /// `Dr. Mariam Adel`
  String get chatMockupPharmacistName {
    return Intl.message(
      'Dr. Mariam Adel',
      name: 'chatMockupPharmacistName',
      desc: '',
      args: [],
    );
  }

  /// `Active chat`
  String get chatMockupActiveStatus {
    return Intl.message(
      'Active chat',
      name: 'chatMockupActiveStatus',
      desc: '',
      args: [],
    );
  }

  /// `Paid`
  String get chatMockupPaidStatus {
    return Intl.message(
      'Paid',
      name: 'chatMockupPaidStatus',
      desc: '',
      args: [],
    );
  }

  /// `Payment completed · chat opened`
  String get chatMockupPaymentBanner {
    return Intl.message(
      'Payment completed · chat opened',
      name: 'chatMockupPaymentBanner',
      desc: '',
      args: [],
    );
  }

  /// `I've had trouble sleeping for about two weeks.`
  String get chatMockupPatientMessage {
    return Intl.message(
      'I\'ve had trouble sleeping for about two weeks.',
      name: 'chatMockupPatientMessage',
      desc: '',
      args: [],
    );
  }

  /// `Thanks for sharing. Are you taking any medication at the moment?`
  String get chatMockupPharmacistMessage {
    return Intl.message(
      'Thanks for sharing. Are you taking any medication at the moment?',
      name: 'chatMockupPharmacistMessage',
      desc: '',
      args: [],
    );
  }

  /// `Incoming video call`
  String get chatMockupIncomingCallTitle {
    return Intl.message(
      'Incoming video call',
      name: 'chatMockupIncomingCallTitle',
      desc: '',
      args: [],
    );
  }

  /// `Dr. Mariam is calling...`
  String get chatMockupIncomingCallSubtitle {
    return Intl.message(
      'Dr. Mariam is calling...',
      name: 'chatMockupIncomingCallSubtitle',
      desc: '',
      args: [],
    );
  }

  /// `Join`
  String get chatMockupJoinAction {
    return Intl.message(
      'Join',
      name: 'chatMockupJoinAction',
      desc: '',
      args: [],
    );
  }

  /// `Herbal prescription`
  String get prescriptionMockupTitle {
    return Intl.message(
      'Herbal prescription',
      name: 'prescriptionMockupTitle',
      desc: '',
      args: [],
    );
  }

  /// `RX-2026-00142`
  String get prescriptionMockupCode {
    return Intl.message(
      'RX-2026-00142',
      name: 'prescriptionMockupCode',
      desc: '',
      args: [],
    );
  }

  /// `PDF`
  String get prescriptionMockupPdf {
    return Intl.message(
      'PDF',
      name: 'prescriptionMockupPdf',
      desc: '',
      args: [],
    );
  }

  /// `Chamomile`
  String get prescriptionMockupHerb1 {
    return Intl.message(
      'Chamomile',
      name: 'prescriptionMockupHerb1',
      desc: '',
      args: [],
    );
  }

  /// `1 cup · twice daily`
  String get prescriptionMockupDose1 {
    return Intl.message(
      '1 cup · twice daily',
      name: 'prescriptionMockupDose1',
      desc: '',
      args: [],
    );
  }

  /// `Lemon balm`
  String get prescriptionMockupHerb2 {
    return Intl.message(
      'Lemon balm',
      name: 'prescriptionMockupHerb2',
      desc: '',
      args: [],
    );
  }

  /// `1 cup · at night`
  String get prescriptionMockupDose2 {
    return Intl.message(
      '1 cup · at night',
      name: 'prescriptionMockupDose2',
      desc: '',
      args: [],
    );
  }

  /// `CLINICAL SUMMARY`
  String get prescriptionMockupClinicalSummary {
    return Intl.message(
      'CLINICAL SUMMARY',
      name: 'prescriptionMockupClinicalSummary',
      desc: '',
      args: [],
    );
  }

  /// `Allergies`
  String get prescriptionMockupAllergies {
    return Intl.message(
      'Allergies',
      name: 'prescriptionMockupAllergies',
      desc: '',
      args: [],
    );
  }

  /// `Conditions`
  String get prescriptionMockupConditions {
    return Intl.message(
      'Conditions',
      name: 'prescriptionMockupConditions',
      desc: '',
      args: [],
    );
  }

  /// `Medications`
  String get prescriptionMockupMedications {
    return Intl.message(
      'Medications',
      name: 'prescriptionMockupMedications',
      desc: '',
      args: [],
    );
  }

  /// `Share my summary with this pharmacist`
  String get prescriptionMockupShareSummary {
    return Intl.message(
      'Share my summary with this pharmacist',
      name: 'prescriptionMockupShareSummary',
      desc: '',
      args: [],
    );
  }

  /// `اختر لغتك`
  String get chooseLanguageTitleAr {
    return Intl.message(
      'اختر لغتك',
      name: 'chooseLanguageTitleAr',
      desc: '',
      args: [],
    );
  }

  /// `Choose your language`
  String get chooseLanguageTitleEn {
    return Intl.message(
      'Choose your language',
      name: 'chooseLanguageTitleEn',
      desc: '',
      args: [],
    );
  }

  /// `العربية`
  String get languageArabicOption {
    return Intl.message(
      'العربية',
      name: 'languageArabicOption',
      desc: '',
      args: [],
    );
  }

  /// `English`
  String get languageEnglishOption {
    return Intl.message(
      'English',
      name: 'languageEnglishOption',
      desc: '',
      args: [],
    );
  }

  /// `يمكنك تغيير اللغة لاحقًا من الإعدادات`
  String get chooseLanguageHintAr {
    return Intl.message(
      'يمكنك تغيير اللغة لاحقًا من الإعدادات',
      name: 'chooseLanguageHintAr',
      desc: '',
      args: [],
    );
  }

  /// `You can change it later in Settings`
  String get chooseLanguageHintEn {
    return Intl.message(
      'You can change it later in Settings',
      name: 'chooseLanguageHintEn',
      desc: '',
      args: [],
    );
  }
}

class AppLocalizationDelegate extends LocalizationsDelegate<S> {
  const AppLocalizationDelegate();

  List<Locale> get supportedLocales {
    return const <Locale>[
      Locale.fromSubtags(languageCode: 'en'),
      Locale.fromSubtags(languageCode: 'ar'),
    ];
  }

  @override
  bool isSupported(Locale locale) => _isSupported(locale);
  @override
  Future<S> load(Locale locale) => S.load(locale);
  @override
  bool shouldReload(AppLocalizationDelegate old) => false;

  bool _isSupported(Locale locale) {
    for (var supportedLocale in supportedLocales) {
      if (supportedLocale.languageCode == locale.languageCode) {
        return true;
      }
    }
    return false;
  }
}
