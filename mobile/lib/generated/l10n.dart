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

  /// `Welcome back`
  String get loginWelcomeTitle {
    return Intl.message(
      'Welcome back',
      name: 'loginWelcomeTitle',
      desc: '',
      args: [],
    );
  }

  /// `Log in to follow your consultations and manage your health summary.`
  String get loginWelcomeSubtitle {
    return Intl.message(
      'Log in to follow your consultations and manage your health summary.',
      name: 'loginWelcomeSubtitle',
      desc: '',
      args: [],
    );
  }

  /// `Email or phone number`
  String get emailOrPhoneLabel {
    return Intl.message(
      'Email or phone number',
      name: 'emailOrPhoneLabel',
      desc: '',
      args: [],
    );
  }

  /// `name@example.com or +20...`
  String get emailOrPhoneHint {
    return Intl.message(
      'name@example.com or +20...',
      name: 'emailOrPhoneHint',
      desc: '',
      args: [],
    );
  }

  /// ``
  String get emailOrPhoneLtrHelper {
    return Intl.message('', name: 'emailOrPhoneLtrHelper', desc: '', args: []);
  }

  /// `Password`
  String get passwordLabel {
    return Intl.message('Password', name: 'passwordLabel', desc: '', args: []);
  }

  /// `Enter your password`
  String get passwordHint {
    return Intl.message(
      'Enter your password',
      name: 'passwordHint',
      desc: '',
      args: [],
    );
  }

  /// `Log in`
  String get loginButton {
    return Intl.message('Log in', name: 'loginButton', desc: '', args: []);
  }

  /// `Try again`
  String get tryAgainButton {
    return Intl.message(
      'Try again',
      name: 'tryAgainButton',
      desc: '',
      args: [],
    );
  }

  /// `We couldn't log you in`
  String get loginErrorTitle {
    return Intl.message(
      'We couldn\'t log you in',
      name: 'loginErrorTitle',
      desc: '',
      args: [],
    );
  }

  /// `The email, phone number or password is incorrect. Check them and try again.`
  String get loginErrorDescription {
    return Intl.message(
      'The email, phone number or password is incorrect. Check them and try again.',
      name: 'loginErrorDescription',
      desc: '',
      args: [],
    );
  }

  /// `This app is for patients. Pharmacists sign in through the web portal.`
  String get patientPortalNotice {
    return Intl.message(
      'This app is for patients. Pharmacists sign in through the web portal.',
      name: 'patientPortalNotice',
      desc: '',
      args: [],
    );
  }

  /// `New to Shifa? `
  String get newToShifa {
    return Intl.message(
      'New to Shifa? ',
      name: 'newToShifa',
      desc: '',
      args: [],
    );
  }

  /// `Create account`
  String get createAccount {
    return Intl.message(
      'Create account',
      name: 'createAccount',
      desc: '',
      args: [],
    );
  }

  /// `Please enter your email or phone number`
  String get emailOrPhoneRequiredError {
    return Intl.message(
      'Please enter your email or phone number',
      name: 'emailOrPhoneRequiredError',
      desc: '',
      args: [],
    );
  }

  /// `Please enter your password`
  String get passwordRequiredError {
    return Intl.message(
      'Please enter your password',
      name: 'passwordRequiredError',
      desc: '',
      args: [],
    );
  }

  /// `Step {current} of {total}`
  String stepOfTotal(int current, int total) {
    return Intl.message(
      'Step $current of $total',
      name: 'stepOfTotal',
      desc: '',
      args: [current, total],
    );
  }

  /// `Create your account`
  String get createAccountTitle {
    return Intl.message(
      'Create your account',
      name: 'createAccountTitle',
      desc: '',
      args: [],
    );
  }

  /// `Account details`
  String get accountDetailsSubtitle {
    return Intl.message(
      'Account details',
      name: 'accountDetailsSubtitle',
      desc: '',
      args: [],
    );
  }

  /// `Personal details`
  String get personalDetailsTitle {
    return Intl.message(
      'Personal details',
      name: 'personalDetailsTitle',
      desc: '',
      args: [],
    );
  }

  /// `As shown on your national ID`
  String get personalDetailsSubtitle {
    return Intl.message(
      'As shown on your national ID',
      name: 'personalDetailsSubtitle',
      desc: '',
      args: [],
    );
  }

  /// `Clinical basics`
  String get clinicalBasicsTitle {
    return Intl.message(
      'Clinical basics',
      name: 'clinicalBasicsTitle',
      desc: '',
      args: [],
    );
  }

  /// `Helps your pharmacist advise safely`
  String get clinicalBasicsSubtitle {
    return Intl.message(
      'Helps your pharmacist advise safely',
      name: 'clinicalBasicsSubtitle',
      desc: '',
      args: [],
    );
  }

  /// `Emergency contact`
  String get emergencyContactTitle {
    return Intl.message(
      'Emergency contact',
      name: 'emergencyContactTitle',
      desc: '',
      args: [],
    );
  }

  /// `Then review and create your account`
  String get emergencyContactSubtitle {
    return Intl.message(
      'Then review and create your account',
      name: 'emergencyContactSubtitle',
      desc: '',
      args: [],
    );
  }

  /// `Email`
  String get emailLabel {
    return Intl.message('Email', name: 'emailLabel', desc: '', args: []);
  }

  /// `name@example.com`
  String get emailHint {
    return Intl.message(
      'name@example.com',
      name: 'emailHint',
      desc: '',
      args: [],
    );
  }

  /// `Phone number`
  String get phoneNumberLabel {
    return Intl.message(
      'Phone number',
      name: 'phoneNumberLabel',
      desc: '',
      args: [],
    );
  }

  /// `+20 10 1234 5678`
  String get phoneNumberHint {
    return Intl.message(
      '+20 10 1234 5678',
      name: 'phoneNumberHint',
      desc: '',
      args: [],
    );
  }

  /// `Create a password`
  String get createPasswordHint {
    return Intl.message(
      'Create a password',
      name: 'createPasswordHint',
      desc: '',
      args: [],
    );
  }

  /// `Confirm password`
  String get confirmPasswordLabel {
    return Intl.message(
      'Confirm password',
      name: 'confirmPasswordLabel',
      desc: '',
      args: [],
    );
  }

  /// `Re-enter your password`
  String get reEnterPasswordHint {
    return Intl.message(
      'Re-enter your password',
      name: 'reEnterPasswordHint',
      desc: '',
      args: [],
    );
  }

  /// `Nothing is sent until you review your details and tap `
  String get registerInfoBannerPrefix {
    return Intl.message(
      'Nothing is sent until you review your details and tap ',
      name: 'registerInfoBannerPrefix',
      desc: '',
      args: [],
    );
  }

  /// ` in step 4.`
  String get registerInfoBannerSuffix {
    return Intl.message(
      ' in step 4.',
      name: 'registerInfoBannerSuffix',
      desc: '',
      args: [],
    );
  }

  /// `Continue`
  String get continueButton {
    return Intl.message('Continue', name: 'continueButton', desc: '', args: []);
  }

  /// `Continue to review`
  String get continueToReviewButton {
    return Intl.message(
      'Continue to review',
      name: 'continueToReviewButton',
      desc: '',
      args: [],
    );
  }

  /// `Back`
  String get backButton {
    return Intl.message('Back', name: 'backButton', desc: '', args: []);
  }

  /// `Already have an account?`
  String get alreadyHaveAccount {
    return Intl.message(
      'Already have an account?',
      name: 'alreadyHaveAccount',
      desc: '',
      args: [],
    );
  }

  /// `Is this your account?`
  String get isThisYourAccount {
    return Intl.message(
      'Is this your account?',
      name: 'isThisYourAccount',
      desc: '',
      args: [],
    );
  }

  /// `Log in`
  String get logInAction {
    return Intl.message('Log in', name: 'logInAction', desc: '', args: []);
  }

  /// `Full name`
  String get fullNameLabel {
    return Intl.message('Full name', name: 'fullNameLabel', desc: '', args: []);
  }

  /// `Your full name, Arabic or English`
  String get fullNameHint {
    return Intl.message(
      'Your full name, Arabic or English',
      name: 'fullNameHint',
      desc: '',
      args: [],
    );
  }

  /// `National ID`
  String get nationalIdLabel {
    return Intl.message(
      'National ID',
      name: 'nationalIdLabel',
      desc: '',
      args: [],
    );
  }

  /// `e.g. 29801011234568`
  String get nationalIdHint {
    return Intl.message(
      'e.g. 29801011234568',
      name: 'nationalIdHint',
      desc: '',
      args: [],
    );
  }

  /// `Date of birth`
  String get dateOfBirthLabel {
    return Intl.message(
      'Date of birth',
      name: 'dateOfBirthLabel',
      desc: '',
      args: [],
    );
  }

  /// `DD / MM / YYYY`
  String get dateOfBirthHint {
    return Intl.message(
      'DD / MM / YYYY',
      name: 'dateOfBirthHint',
      desc: '',
      args: [],
    );
  }

  /// `Gender`
  String get genderLabel {
    return Intl.message('Gender', name: 'genderLabel', desc: '', args: []);
  }

  /// `Male`
  String get maleOption {
    return Intl.message('Male', name: 'maleOption', desc: '', args: []);
  }

  /// `Female`
  String get femaleOption {
    return Intl.message('Female', name: 'femaleOption', desc: '', args: []);
  }

  /// `Blood type`
  String get bloodTypeLabel {
    return Intl.message(
      'Blood type',
      name: 'bloodTypeLabel',
      desc: '',
      args: [],
    );
  }

  /// `I don't know my blood type`
  String get dontKnowBloodType {
    return Intl.message(
      'I don\'t know my blood type',
      name: 'dontKnowBloodType',
      desc: '',
      args: [],
    );
  }

  /// `Weight`
  String get weightLabel {
    return Intl.message('Weight', name: 'weightLabel', desc: '', args: []);
  }

  /// `Height`
  String get heightLabel {
    return Intl.message('Height', name: 'heightLabel', desc: '', args: []);
  }

  /// `You can add allergies, conditions and medications later in your health summary.`
  String get clinicalInfoBanner {
    return Intl.message(
      'You can add allergies, conditions and medications later in your health summary.',
      name: 'clinicalInfoBanner',
      desc: '',
      args: [],
    );
  }

  /// `Contact name`
  String get contactNameLabel {
    return Intl.message(
      'Contact name',
      name: 'contactNameLabel',
      desc: '',
      args: [],
    );
  }

  /// `e.g. Ahmed Mostafa`
  String get contactNameHint {
    return Intl.message(
      'e.g. Ahmed Mostafa',
      name: 'contactNameHint',
      desc: '',
      args: [],
    );
  }

  /// `Contact phone`
  String get contactPhoneLabel {
    return Intl.message(
      'Contact phone',
      name: 'contactPhoneLabel',
      desc: '',
      args: [],
    );
  }

  /// `Review your details`
  String get reviewYourDetailsTitle {
    return Intl.message(
      'Review your details',
      name: 'reviewYourDetailsTitle',
      desc: '',
      args: [],
    );
  }

  /// `Tap Edit to change a step`
  String get tapEditToChangeStep {
    return Intl.message(
      'Tap Edit to change a step',
      name: 'tapEditToChangeStep',
      desc: '',
      args: [],
    );
  }

  /// `Edit`
  String get editAction {
    return Intl.message('Edit', name: 'editAction', desc: '', args: []);
  }

  /// `1 · ACCOUNT`
  String get sectionAccount {
    return Intl.message(
      '1 · ACCOUNT',
      name: 'sectionAccount',
      desc: '',
      args: [],
    );
  }

  /// `2 · PERSONAL`
  String get sectionPersonal {
    return Intl.message(
      '2 · PERSONAL',
      name: 'sectionPersonal',
      desc: '',
      args: [],
    );
  }

  /// `3 · CLINICAL`
  String get sectionClinical {
    return Intl.message(
      '3 · CLINICAL',
      name: 'sectionClinical',
      desc: '',
      args: [],
    );
  }

  /// `Create account`
  String get createAccountButton {
    return Intl.message(
      'Create account',
      name: 'createAccountButton',
      desc: '',
      args: [],
    );
  }

  /// `Not provided`
  String get notProvided {
    return Intl.message(
      'Not provided',
      name: 'notProvided',
      desc: '',
      args: [],
    );
  }

  /// `We couldn't create your account`
  String get couldntCreateAccountTitle {
    return Intl.message(
      'We couldn\'t create your account',
      name: 'couldntCreateAccountTitle',
      desc: '',
      args: [],
    );
  }

  /// `Your other details are saved. Fix the highlighted field, then continue to review.`
  String get couldntCreateAccountSubtitle {
    return Intl.message(
      'Your other details are saved. Fix the highlighted field, then continue to review.',
      name: 'couldntCreateAccountSubtitle',
      desc: '',
      args: [],
    );
  }

  /// `Please enter your email address`
  String get emailRequiredError {
    return Intl.message(
      'Please enter your email address',
      name: 'emailRequiredError',
      desc: '',
      args: [],
    );
  }

  /// `Please enter a valid email address`
  String get invalidEmailError {
    return Intl.message(
      'Please enter a valid email address',
      name: 'invalidEmailError',
      desc: '',
      args: [],
    );
  }

  /// `Please enter your phone number`
  String get phoneRequiredError {
    return Intl.message(
      'Please enter your phone number',
      name: 'phoneRequiredError',
      desc: '',
      args: [],
    );
  }

  /// `Please enter a valid phone number`
  String get invalidPhoneError {
    return Intl.message(
      'Please enter a valid phone number',
      name: 'invalidPhoneError',
      desc: '',
      args: [],
    );
  }

  /// `Password must be at least 8 characters`
  String get passwordLengthError {
    return Intl.message(
      'Password must be at least 8 characters',
      name: 'passwordLengthError',
      desc: '',
      args: [],
    );
  }

  /// `Please confirm your password`
  String get confirmPasswordRequiredError {
    return Intl.message(
      'Please confirm your password',
      name: 'confirmPasswordRequiredError',
      desc: '',
      args: [],
    );
  }

  /// `Passwords do not match`
  String get passwordsDoNotMatchError {
    return Intl.message(
      'Passwords do not match',
      name: 'passwordsDoNotMatchError',
      desc: '',
      args: [],
    );
  }

  /// `Please enter your full name`
  String get fullNameRequiredError {
    return Intl.message(
      'Please enter your full name',
      name: 'fullNameRequiredError',
      desc: '',
      args: [],
    );
  }

  /// `Please enter your national ID`
  String get nationalIdRequiredError {
    return Intl.message(
      'Please enter your national ID',
      name: 'nationalIdRequiredError',
      desc: '',
      args: [],
    );
  }

  /// `Please enter a valid 14-digit national ID`
  String get invalidNationalIdError {
    return Intl.message(
      'Please enter a valid 14-digit national ID',
      name: 'invalidNationalIdError',
      desc: '',
      args: [],
    );
  }

  /// `Please select your date of birth`
  String get dateOfBirthRequiredError {
    return Intl.message(
      'Please select your date of birth',
      name: 'dateOfBirthRequiredError',
      desc: '',
      args: [],
    );
  }

  /// `Please select your gender`
  String get genderRequiredError {
    return Intl.message(
      'Please select your gender',
      name: 'genderRequiredError',
      desc: '',
      args: [],
    );
  }

  /// `0.0`
  String get weightHint {
    return Intl.message('0.0', name: 'weightHint', desc: '', args: []);
  }

  /// `0`
  String get heightHint {
    return Intl.message('0', name: 'heightHint', desc: '', args: []);
  }

  /// `kg`
  String get kgUnit {
    return Intl.message('kg', name: 'kgUnit', desc: '', args: []);
  }

  /// `cm`
  String get cmUnit {
    return Intl.message('cm', name: 'cmUnit', desc: '', args: []);
  }

  /// `Invalid number`
  String get invalidNumberError {
    return Intl.message(
      'Invalid number',
      name: 'invalidNumberError',
      desc: '',
      args: [],
    );
  }

  /// `Contact name`
  String get emergencyContactNameLabel {
    return Intl.message(
      'Contact name',
      name: 'emergencyContactNameLabel',
      desc: '',
      args: [],
    );
  }

  /// `Ahmed Mostafa`
  String get emergencyContactNameHint {
    return Intl.message(
      'Ahmed Mostafa',
      name: 'emergencyContactNameHint',
      desc: '',
      args: [],
    );
  }

  /// `Contact phone`
  String get emergencyContactPhoneLabel {
    return Intl.message(
      'Contact phone',
      name: 'emergencyContactPhoneLabel',
      desc: '',
      args: [],
    );
  }

  /// `+20 108 765 4321`
  String get emergencyContactPhoneHint {
    return Intl.message(
      '+20 108 765 4321',
      name: 'emergencyContactPhoneHint',
      desc: '',
      args: [],
    );
  }

  /// `1 · ACCOUNT`
  String get accountSectionTitle {
    return Intl.message(
      '1 · ACCOUNT',
      name: 'accountSectionTitle',
      desc: '',
      args: [],
    );
  }

  /// `2 · PERSONAL`
  String get personalSectionTitle {
    return Intl.message(
      '2 · PERSONAL',
      name: 'personalSectionTitle',
      desc: '',
      args: [],
    );
  }

  /// `3 · CLINICAL`
  String get clinicalSectionTitle {
    return Intl.message(
      '3 · CLINICAL',
      name: 'clinicalSectionTitle',
      desc: '',
      args: [],
    );
  }

  /// `Edit`
  String get editButton {
    return Intl.message('Edit', name: 'editButton', desc: '', args: []);
  }

  /// `Blood`
  String get bloodLabel {
    return Intl.message('Blood', name: 'bloodLabel', desc: '', args: []);
  }

  /// `Home`
  String get navHome {
    return Intl.message('Home', name: 'navHome', desc: '', args: []);
  }

  /// `Pharmacists`
  String get navPharmacists {
    return Intl.message(
      'Pharmacists',
      name: 'navPharmacists',
      desc: '',
      args: [],
    );
  }

  /// `Consultations`
  String get navConsultations {
    return Intl.message(
      'Consultations',
      name: 'navConsultations',
      desc: '',
      args: [],
    );
  }

  /// `Profile`
  String get navProfile {
    return Intl.message('Profile', name: 'navProfile', desc: '', args: []);
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
